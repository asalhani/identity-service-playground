using IdentityServer4.Events;
using IdentityServer4.Quickstart.UI;
using IdentityServer4.Services;
using IdentityServer4.Extensions;
using IdentityService.Models;
using IdentityService.Models.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Stores;
using IdentityService.Helpers;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Options;
using IdentityService.Models.Configurations;
using IdentityService.Business.Abstract;
using IdentityService.Business;
using IdentityService.Models.IdentiryServerEvents;
using System.Security.Claims;
using IdentityModel;
using PlatformCommons.PlatformService.Abstractions.Notification;
using PlatformCommons.PlatformService.Abstractions.Dto;
using IdentityService.Helpers.Abstract;
using Microsoft.Extensions.Caching.Distributed;

namespace IdentityService.Api
{
    [Produces("application/json")]
    [Route("authentication")]
    public class AuthenticationController : BaseApiController
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEventService _events;
        //private readonly IClientStore _clientStore;
        private readonly IAccountManager _accountManager;
        private readonly IEmailNotification _emailNotification;
        private readonly ISmsNotification _smsNotification;
        private readonly AppSettings _settings;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IDistributedCache _distributedCache;

        public AuthenticationController(IIdentityServerInteractionService interaction,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEventService events,
            ICaptchaValidator captchaValidator,
            IAccountManager accountManager,
            IOptions<AppSettings> settings,
            IEmailNotification emailNotification,
            ISmsNotification smsNotification,
            IDistributedCache distributedCache,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _interaction = interaction;
            _userManager = userManager;
            _signInManager = signInManager;
            _events = events;
            _captchaValidator = captchaValidator;
            _accountManager = accountManager;
            _emailNotification = emailNotification;
            _smsNotification = smsNotification;
            _settings = settings.Value;
            _claimsFactory = claimsFactory;
            _distributedCache = distributedCache;
        }

        [HttpPost("sign-in")]
        public async Task<ApiResultBase> SignIn([FromBody]SignInInfoInput model)
        {
            ValidateModelState();
            if (!_captchaValidator.Validate(model.CaptchaString))
            {
                return new SignInOutput() { Success = false, IsInvalidCaptcha = true };
            }

            //// check if we are in the context of an authorization request
            //var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // validate username/password against in-memory store
            var result = await _signInManager.PasswordSignInAsync(model.LoginName, model.Password, true, true);
            if (result.Succeeded || (result.RequiresTwoFactor && !_settings.UserAccount.IsOtpEnabled))
            {

                var user = await _userManager.FindByNameAsync(model.LoginName);

                //check if password has expired
                if (_accountManager.IsUserPasswordExpired(user))
                {
                    var expiredPasswordToken = await _accountManager.GenerateExpiredPasswordResetTokenAsync(user);
                    return new SignInOutput { Success = false, IsPasswordExpired = true, ChangeExpiredPasswordToken = expiredPasswordToken };
                }

                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

                // user claims factory to load all AspNetIdentity user claims and issue the authentication cookie with subject ID and username and other identity server claims
                var princ = await _claimsFactory.CreateAsync(user);
                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, princ);
                await _userManager.ResetAccessFailedCountAsync(user);
                return new SignInOutput() { Success = true };
            }
            else if (result.RequiresTwoFactor)
            {
                var user = await _userManager.FindByNameAsync(model.LoginName);
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone");

                //Send the SMS
                await OTPNotification(user, _settings.Notifications.Templates.SmsSignInOtp, token);

                var output = new SignInOutput()
                {
                    Success = false,
                    IsOtpRequired = true,
                    MobileNumberFraction = user.PhoneNumber.Substring(user.PhoneNumber.Length - 4),
                    AttemptsTimeoutInMintutes = _settings.UserAccount.OtpAttemptsTimeoutInMinutes
                };
#if DEBUG
                output.Issues = token;
#endif
                return output;
            }
            else if (result.IsLockedOut)
            {
                return new SignInOutput() { Success = false, IsAccountLocked = true };
            }

            await _events.RaiseAsync(new UserLoginFailureEvent(model.LoginName, "invalid credentials"));
            return new SignInOutput() { Success = false, IsInvalidUserInfo = true };
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ReSendOTP()
        {

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                return BadRequest(new ResendOTPOutput {
                    Success = false,
                    IsSessionExpired = true,
                    Error = "Session Expired."
                });
            }

            if (!CanUserResendOTP(user))
            {
                return BadRequest(new ResendOTPOutput
                {
                    Success = false,
                    IsSessionExpired = true,
                    Error = "Resend request failed, please try again."
                });
            }

            var userSecurityStamp = await _userManager.UpdateSecurityStampAsync(user);
            string token = "";

            if (!userSecurityStamp.Succeeded)
            {
                return BadRequest(new ResendOTPOutput
                {
                    Success = false,
                    IsSessionExpired = true,
                    Error = "Resend request failed, please try again."
                });
            }
            else
            {
                await _signInManager.RefreshSignInAsync(user);
                token = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone");
                //Send the SMS
                await OTPNotification(user, _settings.Notifications.Templates.SmsSignInOtp, token);
                SetUserResendOTPFlag(user);

                return Ok(new ResendOTPOutput {
                    Success = true
                });
            }
        }

        private bool CanUserResendOTP(ApplicationUser user)
        {
            var resend_flag = _distributedCache.GetInt32($"OTP_Resend_Flag_{user.Id}");

            //allow only if flag is expired
            return resend_flag == null;
        }

        private void SetUserResendOTPFlag(ApplicationUser user)
        {
            _distributedCache.SetInt32($"OTP_Resend_Flag_{user.Id}", 1, new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.UserAccount.OtpAllowResendAfterSeconds)));
        }

        private async Task OTPNotification(ApplicationUser user, int template, string token)
        {
            await _smsNotification.SendSms(new SendSmsInput()
            {
                Number = user.PhoneNumber,
                TemplateId = template,
                SmsParams = new Dictionary<string, string>() { { "ActivationCode", token } }
            });
            SetUserResendOTPFlag(user);
        }

        [HttpPost("verify-otp")]
        public async Task<ApiResultBase> VerifyOtp([FromBody]OtpInput model)
        {
            ValidateModelState();
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                await _events.RaiseAsync(new OtpValidationFailure("Unable to retrieve two-factor authentication user session for otp verification."));
                return new OtpOutput() { Success = false, IsOtpExpired = true };
            }
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
            var codeVerified = await IsValidOTPAsync(user, model.OtpValue);
            var otpRequestType = GetOtpRequestType(result.Principal.Claims);
            switch (otpRequestType)
            {
                case OtpRequestTypes.ResetPassword:
                    return await HandleOtpForResetPassword(result.Principal.Claims, codeVerified, user);
                case OtpRequestTypes.SignIn:
                default:
                    return await HandleOtpForLogin(result.Principal.Claims, codeVerified, user, model.OtpValue);
            }
        }

        [HttpGet("sign-out")]
        public async Task<ApiResultBase> Logout(string logoutId)
        {
            //Due to nature of identity server being an isolated entity, this leads to the fact that a siqn-out request can be 
            //initialized from any source. So the implemetation assumes that the user should be prompted if he is not already signed in 
            var result = new SignOutOutput();
            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                result.IsShowPrompt = false;
            }

            var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);
            result.ClientName = string.IsNullOrEmpty(logoutContext?.ClientName) ? logoutContext?.ClientId : logoutContext?.ClientName;
            if (logoutContext?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                result.IsShowPrompt = false;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            if (result.IsShowPrompt)
                return await Task.FromResult(result);

            // if the request for logout was properly authenticated from IdentityServer, then
            // we don't need to show the prompt and can just log the user out directly.
            return await LogoutAssured(logoutId);
        }

        [HttpPost("sign-out")]
        public async Task<ApiResultBase> LogoutAssured(string logoutId)
        {
            var result = new SignOutOutput() { IsShowPrompt = false };
            //**************************************************************************
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);
            result.ClientName = string.IsNullOrEmpty(logoutContext?.ClientName) ? logoutContext?.ClientId : logoutContext?.ClientName;
            result.PostLogoutRedirectUri = logoutContext?.PostLogoutRedirectUri;

            if (User?.Identity.IsAuthenticated == true)
            {
                //var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                //if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                //{
                //    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                //    if (providerSupportsSignout)
                //    {
                //        if (logoutId == null)
                //        {
                //            // if there's no current logout context, we need to create one
                //            // this captures necessary info from the current logged in user
                //            // before we signout and redirect away to the external IdP for signout
                //            logoutId = await _interaction.CreateLogoutContextAsync();
                //        }

                //        vm.ExternalAuthenticationScheme = idp;
                //    }
                //}

                // delete local authentication cookie
                //await HttpContext.SignOutAsync();
                //await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                var user = await _userManager.GetUserAsync(User);
                await _userManager.UpdateSecurityStampAsync(user);
                await _signInManager.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            return result;
        }

        private async Task<bool> IsValidOTPAsync(ApplicationUser user, string code)
        {
            var codeVerified = await _userManager.VerifyTwoFactorTokenAsync(user, "Phone", code);
            bool reachedMaxAttemps = false;

            if (!codeVerified)
            {
                var opt = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.UserAccount.OtpAttemptsTimeoutInMinutes));

                int attemps = _distributedCache.GetInt32($"otpAttemps_{user.SecurityStamp}") ?? 0;
                if (attemps < 3)
                {
                    _distributedCache.SetInt32($"otpAttemps_{user.SecurityStamp}", attemps + 1, opt);
                }
                else
                {
                    reachedMaxAttemps = true;
                    //to prevent further otp login attemps:
                    await _userManager.GetSecurityStampAsync(user);
                }

            }

            return codeVerified && !reachedMaxAttemps;
        }

        private OtpRequestTypes GetOtpRequestType(IEnumerable<Claim> claims)
        {
            if (claims.Any(x => x.Type == "_password"))
                return OtpRequestTypes.ResetPassword;
            return OtpRequestTypes.SignIn;
        }

        private async Task<ApiResultBase> HandleOtpForResetPassword(IEnumerable<Claim> principalClaims, bool isCodeVerified, ApplicationUser user)
        {
            if (isCodeVerified)
            {
                string sPassword = principalClaims.FirstOrDefault(x => x.Type == "_password")?.Value;
                string sEmailToken = principalClaims.FirstOrDefault(x => x.Type == "_emailToken")?.Value;
                if (string.IsNullOrEmpty(sPassword) || string.IsNullOrEmpty(sEmailToken))
                    throw new Exception("Either password or email token was not found in the two factor auth cookie.");
                await _accountManager.ResetPassword(sPassword, sEmailToken, user);
                return await Task.FromResult(new OtpOutput() { Success = true });
            }
            else
            {
                return await Task.FromResult(new OtpOutput() { Success = false, IsInvalidOtp = true });
            }
        }
        private async Task<ApiResultBase> HandleOtpForLogin(IEnumerable<Claim> principalClaims, bool isCodeVerified, ApplicationUser user, string otpValue)
        {
            if (isCodeVerified)
            {
                //check if password has expired
                if (_accountManager.IsUserPasswordExpired(user))
                {
                    var expiredPasswordToken = await _accountManager.GenerateExpiredPasswordResetTokenAsync(user);
                    return new SignInOtpOutput { Success = false, IsPasswordExpired = true, ChangeExpiredPasswordToken = expiredPasswordToken };
                }

                var result = await _signInManager.TwoFactorSignInAsync("Phone", otpValue, false, false);
                if (result.Succeeded)
                {
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));
                    await _userManager.ResetAccessFailedCountAsync(user);
                    return new SignInOtpOutput() { Success = true };
                }
                else if (result.IsLockedOut)
                {
                    return new SignInOtpOutput() { Success = false, IsAccountLocked = true };
                }
                else
                    throw new InvalidOperationException($"Uexpected result from TwoFactorSignInAsync: {result.IsLockedOut} {result.IsNotAllowed} {result.RequiresTwoFactor} {result.Succeeded}");
            }
            else
            {
                user.AccessFailedCount++;

                if (user.AccessFailedCount >= _settings.UserAccount.MaxFailedAccessAttemptsBeforeLockout)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(_settings.UserAccount.LockoutTimeSpanInMinutes);
                    user.AccessFailedCount = 0;
                    await _userManager.UpdateAsync(user);
                    return new SignInOtpOutput() { Success = false, IsAccountLocked = true };
                }
                else
                {
                    await _userManager.UpdateAsync(user);
                    return new SignInOtpOutput() { Success = false, IsInvalidOtp = true };
                }
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ForgotPasswordOutput> ForgotPasswordProcess([FromBody]ForgotPasswordInput model)
        {
            ValidateModelState();
            if (!_captchaValidator.Validate(model.CaptchaString))
            {
                return new ForgotPasswordOutput() { Success = false, IsInvalidCaptcha = true };
            }

            try
            {
                var result = _accountManager.HandleForgottenPasswordRequest(model);
                return await Task.FromResult(new ForgotPasswordOutput() { Info = result, Success = true, IsInvalidCaptcha = false });
            }
            catch (BusinessException ex)
            {
                await _events.RaiseAsync(new ForgotPasswordFailure(ex.Message));
                return await Task.FromResult(new ForgotPasswordOutput());
            }
        }

        [HttpGet("validate-email-token")]
        public async Task<bool> ValidateEmailToken(string token)
        {
            try
            {
                _accountManager.ValidateEmailToken(token);
                return true;
            }
            catch (BusinessException ex)
            {
                await _events.RaiseAsync(new ForgotPasswordFailure(ex.Message));
                return false;
            }
        }

        [HttpPost("reset-password")]
        public async Task<ResetPasswordOuput> ResetPassword([FromBody]ResetPasswordInput model)
        {
            //if (!_captchaValidator.Validate(model.CaptchaString))
            //{
            //    //This return type will be used in case text based captcha is used. But for the case reCAPTCHA an exception should be trigerred
            //    //return new xxxOutput() { Success = false, InvalidCaptcha = true };
            //    throw new InvalidDataException("Invalid reCAPTCHA");
            //}

            if (!_captchaValidator.Validate(model.CaptchaString))
            {
                return new ResetPasswordOuput() { Success = false, IsInvalidCaptcha = true };
            }

            try
            {
                //TODO: What if the user is locked??
                var ret = _accountManager.ValidateResetPasswordRequest(model);
                ///*******************************************************************************************
                ////*** refere to "Identity/src/Identity/SignInManager.cs" implementation on github.com
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(IdentityConstants.TwoFactorUserIdScheme);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, ret.Item1.UserId));
                claimsIdentity.AddClaim(new Claim("_password", model.Password));
                claimsIdentity.AddClaim(new Claim("_emailToken", model.EmailToken));
                await HttpContext.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, new ClaimsPrincipal(claimsIdentity));
                ///*******************************************************************************************
                var token = await _userManager.GenerateTwoFactorTokenAsync(ret.Item2, "Phone");

                //Send the SMS
                await OTPNotification(ret.Item2, _settings.Notifications.Templates.SmsResetPasswordOtp, token);

                var output = new ResetPasswordOuput()
                {
                    Success = true,
                    MobileNumberFraction = ret.Item2.PhoneNumber.Substring(ret.Item2.PhoneNumber.Length - 4),
                    AttemptsTimeoutInMintutes = _settings.UserAccount.OtpAttemptsTimeoutInMinutes
                };
#if DEBUG
                output.Issues = token;
#endif
                return await Task.FromResult(output);
            }
            catch (BusinessException ex)
            {
                //await _events.RaiseAsync(new ForgotPasswordFailure(ex.Message));
                return await Task.FromResult(new ResetPasswordOuput() { Success = false, Issues = ex.Message });
            }
        }

        [HttpPost("change-expired-password")]
        public async Task<ChangeExpiredPasswordOutput> ChangeExpiredPassword([FromBody]ChangeExpiredPasswordInput model)
        {
            ValidateModelState();
            if (!_captchaValidator.Validate(model.CaptchaString))
            {
                //This return type will be used in case text based captcha is used. But for the case reCAPTCHA an exception should be trigerred
                //return new xxxOutput() { Success = false, InvalidCaptcha = true };
                //throw new InvalidDataException("Invalid reCAPTCHA");
                return await Task.FromResult(new ChangeExpiredPasswordOutput { Success = false, IsInvalidCaptcha = true });
            }

            var user = await _userManager.FindByNameAsync(model.LoginName);

            if (user != null)
            {
                try
                {
                    var changeResult = await _accountManager.ChangeExpiredPasswordAsync(model.NewPassword, model.ChangePasswordToken, user);
                    if (changeResult)
                    {
                        return await Task.FromResult(new ChangeExpiredPasswordOutput { Success = true });
                    }
                }
                catch (BusinessException ex)
                {
                    return await Task.FromResult(new ChangeExpiredPasswordOutput
                    {
                        Success = false,
                        IsInvalidNewPassword = true,
                        InvalidPasswordIssues = ex.Message
                    });
                }
            }


            return await Task.FromResult(new ChangeExpiredPasswordOutput { Success = false, IsInvalidRequest = true });
        }
    }
}

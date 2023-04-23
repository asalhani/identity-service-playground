using IdentityService.Business.Abstract;
using IdentityService.Helpers;
using IdentityService.Models;
using IdentityService.Models.Configurations;
using IdentityService.Models.Dto;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlatformCommons.PlatformService.Abstractions.Dto;
using PlatformCommons.PlatformService.Abstractions.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Api
{
    [Produces("application/json")]
    [Route("public-user-management")]
    public class PublicUserManagementController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppSettings _settings;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly IMemberManager _memberManager;
        private readonly ISmsNotification _smsNotification;

        public PublicUserManagementController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<AppSettings> settings, IMemberManager memberManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, ISmsNotification smsNotification)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _settings = settings.Value;
            _memberManager = memberManager;
            _claimsFactory = claimsFactory;
            _smsNotification = smsNotification;
        }

        [HttpPost("add-user")]
        public ApiResultBase Add([FromBody] UserInput userInput)
        {
            string userId = string.Empty;
            try
            {
                userId = _memberManager.AddPublicUser(userInput);
            }
            catch (Exception ex)
            {
                return new ManageUser() { Success = false, ErrorMessage = ex.Message };
            }
            return HandleResult(userId);
        }

        [HttpPost("sign-in")]
        public async Task<ApiResultBase> SignIn([FromBody]PublicSignInInputDto publicSignInInputDto)
        {
            ValidateModelState();

            var applicationUser = await _userManager.FindByNameAsync(publicSignInInputDto.MobileNumber);

            if (applicationUser == null)
                return new SignInOutput() { Success = false, IsInvalidUserInfo = true };

            await _signInManager.SignInAsync(applicationUser, true, null);

            // Generate OTP
            if (_settings.UserAccount.IsOtpEnabled)
            {

                var token = await _userManager.GenerateTwoFactorTokenAsync(applicationUser, "Phone");

                await _smsNotification.SendSms(new SendSmsInput()
                {
                    Number = publicSignInInputDto.MobileNumber,
                    TemplateId = _settings.Notifications.Templates.SmsSignInOtp,
                    SmsParams = new Dictionary<string, string>() { { "ActivationCode", token } }
                });

            }
            return new SignInOutput() { Success = true, IsOtpRequired = _settings.UserAccount.IsOtpEnabled };
        }

        private ApiResultBase HandleResult(string userId)
        {
            return new ManageUser() { Success = true, Id = userId };
        }

    }
}

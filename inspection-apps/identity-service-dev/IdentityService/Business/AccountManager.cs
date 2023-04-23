using IdentityModel;
using IdentityService.Business.Abstract;
using IdentityService.Business.Abstract.Models;
using IdentityService.DataAccess.Abstract;
using IdentityService.DataAccess.Abstract.Models;
using IdentityService.Models;
using IdentityService.Models.Configurations;
using IdentityService.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace IdentityService.Business
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserMessageManager _userMessageManager;
        private readonly AppSettings _settings;
        private readonly IAspNetPasswordHistoryRepository _aspNetPasswordHistoryDA;
        private readonly IStringLocalizer _l;

        public AccountManager(UserManager<ApplicationUser> userManager,
                              IUserMessageManager userMessageManager,
                              IAspNetPasswordHistoryRepository aspNetPasswordHistoryDA,
                              IOptions<AppSettings> settings, IStringLocalizer l)
        {
            _userManager = userManager;
            _userMessageManager = userMessageManager;
            _aspNetPasswordHistoryDA = aspNetPasswordHistoryDA;
            _settings = settings.Value;
            _l = l;
        }

        public string HandleForgottenPasswordRequest(ForgotPasswordInput model)
        {
            var user = _userManager.FindByEmailAsync(model.Email).Result;
            if (user == null)
                throw new BusinessException($"No user has been retrieved for the given email: {model.Email}.");

            ////check if user account is NOT Active
            //if (!user.IsActive)
            //    throw new BusinessException();

            //check if user account is locked
            // Locked if  value of [LockoutEndDateUtc] proparty is greater than current date/time
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
                throw new BusinessException("User account is locked");

            // make all prior email invitations for current user as expired (109419)

            var userMessage = new UserMessageModel()
            {
                UserId = user.Id,
                UserName = user.UserName
            };

            userMessage.EmailParams.Add(EmailPlaceholders.ReciverAddress, user.Email);
            userMessage.EmailParams.Add(EmailPlaceholders.RootSiteUrl, $"{_settings.IdentityServer.IssuerUri}{_settings.IdentityServer.ResetPasswordUrl}");
            string result = null;
            using (var trans = new TransactionScope())
            {
                result = _userMessageManager.SendUserMessage(userMessage, UserMessageTemplateTypes.ForgotPassword);
                // commit transaction
                trans.Complete();
            }
            return result;
        }

        public void ValidateEmailToken(string token)
        {
            ValidateAndGetEmailToken(token);
        }

        public (UserMessageModel, ApplicationUser) ValidateResetPasswordRequest(ResetPasswordInput model)
        {
            UserMessageModel userMessage = null;
            try
            {
                userMessage = ValidateAndGetEmailToken(model.EmailToken);
            }
            catch (BusinessException ex)
            {
                //Throwing the error as system exception since the step of validation has already been passed with the first landing on reset-password page
                throw new Exception("Token is invalid", ex);
            }

            var user = _userManager.FindByIdAsync(userMessage.UserId).Result;
            if (user == null)
                throw new Exception($"User not found: {userMessage.UserId}");

            ValidatePassword(model.Password, user);
            return (userMessage, user);
        }

        public void ValidatePassword(string password, ApplicationUser user)
        {
            CheckPasswordPolicy(password, user);
            CheckPasswordHistory(password, user);
        }

        public async Task<bool> ChangeExpiredPasswordAsync(string newPassword, string resetPasswordToken, ApplicationUser user)
        {
            if (VerifyExpiredPasswordResetToken(user, resetPasswordToken))
            {
                await ChangeUserPasswordAsync(newPassword, user);
                return true;
            }

            return false;
        }
        public async Task<string> GenerateExpiredPasswordResetTokenAsync(ApplicationUser user)
        {
            // Update the Security Stamp, expired password detected
            await _userManager.UpdateSecurityStampAsync(user);
            return await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "ChangeExpiredPassword");
        }

        private bool VerifyExpiredPasswordResetToken(ApplicationUser user, string token)
        {
            return _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ChangeExpiredPassword", token).Result;
        }

        public async Task ResetPassword(string newPassword, string emailToken, ApplicationUser user)
        {
            await ChangeUserPasswordAsync(newPassword, user);
            _userMessageManager.InvalidateToken(emailToken);

        }

        private UserMessageModel ValidateAndGetEmailToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new BusinessException("Token is null or empty");

            if (!Guid.TryParse(token, out Guid guidToken))
                throw new BusinessException("Invalid token format");

            // try to get token from invitation table
            var userMessage = _userMessageManager.GetByToken(token);
            if (userMessage == null)
                throw new BusinessException("Token was not found");

            // check if token was generated to a particular user
            var userInfo = _userManager.FindByIdAsync(userMessage.UserId);

            // check if token is already has been used
            if (userMessage.IsUsed)
                throw new BusinessException("Token has been used before");

            // check if token is canceled (means that a newer token has been issued for the same user)
            if (userMessage.IsCancelled)
                throw new BusinessException("Token has been overwritten");

            // check if token expired
            TimeSpan ts = DateTime.Now - userMessage.CreatedDate;
            if (ts.TotalMinutes > _settings.UserAccount.UserMessageTokenExpirationInMinutes)
                throw new BusinessException("Token is expired");

            return userMessage;

        }

        private void CheckPasswordPolicy(string password, ApplicationUser user)
        {
            if (password == string.Empty || password.Length < _settings.PasswordPolicy.MinPasswordLength || password.Length > 128)
                throw new BusinessException($"Entered password is not compatible with the defined policy");

            int iTotalPasswordLength = 0, iNumOfMatches;
            //count english letters
            iNumOfMatches = (new Regex(@"[A-Za-z]")).Matches(password).Count;
            if (iNumOfMatches < 1)
                throw new BusinessException($"Password should contain at least one English letter");
            iTotalPasswordLength += iNumOfMatches;

            //count digits and special characters. as per the SRS:  ~!@#$%^&*()-_=+ []{}|;:,.<>/?
            iNumOfMatches = (new Regex(@"[0-9~!@#\$%\^&\*\(\)\-_=\+ \[\]{}\|;:,\.<>\/\?]")).Matches(password).Count;
            if (iNumOfMatches < 1)
                throw new BusinessException($"Password should contain at least one digit or special letter");
            iTotalPasswordLength += iNumOfMatches;

            int iMaxRepititiveCharacters = _settings.PasswordPolicy.MaxConsecutiveLetters;
            int count = 1;
            char prev = password[0];
            for (int i = 1; i < password.Length; i++)
            {
                char current = password[i];
                if (current == prev)
                {
                    count++;
                    if (count > iMaxRepititiveCharacters)
                        throw new BusinessException($"Password should not contain more than 2 consecutive similar characters");
                }
                else
                {
                    prev = current;
                    count = 1;
                }
            }

            if (password.Length != iTotalPasswordLength)
                throw new BusinessException("Entered password is not compatible with the defined policy");

            if (string.Compare(password.ToLower(), user.UserName.ToLower()) == 0)
                throw new BusinessException("Password cannot be the same as the user name");
        }

        private void CheckPasswordHistory(string newpassword, ApplicationUser user)
        {
            //Get Password History
            var userpasswords = _aspNetPasswordHistoryDA.ListByUserId(user.Id);
            if (userpasswords != null && userpasswords.Count > 0)
            {
                foreach (var password in userpasswords)
                {
                    var passwordResult = VerifyHashedPassword(user, password.Password, newpassword);
                    // Return if Password Exists
                    if (passwordResult)
                        throw new BusinessException("Entered password match one of your previous 4 passwords");
                }
            }
        }

        private bool VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            var passwordResult = _userManager.PasswordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);

            if (passwordResult == PasswordVerificationResult.Success)
                return true;
            else
                return false;
        }

        public bool IsUserPasswordExpired(ApplicationUser user)
        {
            //check user password age and compare with policy
            // if LastPasswordChangedDate is null, date min value is assumed which will cause password to expire
            var passwordAge = (DateTime.Now - (user.LastPasswordChangedDate ?? DateTime.MinValue)).Days;
            return passwordAge > _settings.UserAccount.MaxPasswordAgeInDays;
        }

        private async Task ChangeUserPasswordAsync(string newPassword, ApplicationUser user)
        {
            using (TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                ValidatePassword(newPassword, user);

                //change password
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);
                user.LastPasswordChangedDate = DateTime.Now;

                //save password history
                var passwordHistory = new AspNetPasswordHistoryModel
                {
                    UserId = user.Id,
                    Password = user.PasswordHash,
                    DateCreated = user.LastPasswordChangedDate
                };
                _aspNetPasswordHistoryDA.Add(passwordHistory);

                var updateresult = await _userManager.UpdateAsync(user);

                if (updateresult.Succeeded)
                {
                    // Update the Security Stamp
                    await _userManager.UpdateSecurityStampAsync(user);
                    trans.Complete();
                }
                else
                {
                    throw new BusinessException($"Failed to update user: {string.Join(". ", updateresult.Errors.Select(x => x.Description))}");
                }
            }
        }

        public async Task ChangePasswordAsync(UserChangePasswordInput changePasswordInput)
        {
            var user = _userManager.FindByIdAsync(changePasswordInput.UserId).Result;
            if (user == null)
                throw new BusinessException("User does not exists");

            var passCheck = await _userManager.CheckPasswordAsync(user, changePasswordInput.OldPassword);

            if (!passCheck)
                throw new BusinessException("Invalid user name and password");

            await ChangeUserPasswordAsync(changePasswordInput.NewPassword, user);

        }
    }
}

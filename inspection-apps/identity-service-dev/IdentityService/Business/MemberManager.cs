using IdentityService.Business.Abstract;
using IdentityService.DataAccess.Abstract.Models;
using IdentityService.Models;
using IdentityService.Models.Configurations;
using IdentityService.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PlatformCommons.PlatformService.Abstractions.Dto;
using PlatformCommons.PlatformService.Abstractions.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace IdentityService.Business
{
    public class MemberManager : IMemberManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _settings;
        private readonly IUserMessageManager _userMessageManager;
        private readonly IEmailNotification _emailNotification;
        private readonly ISmsNotification _smsNotification;

        public MemberManager(UserManager<ApplicationUser> userManager,
            IOptions<AppSettings> settings,
            IUserMessageManager userMessageManager,
            IEmailNotification emailNotification,
            ISmsNotification smsNotification)
        {
            _userManager = userManager;
            _settings = settings.Value;
            _userMessageManager = userMessageManager;
            _emailNotification = emailNotification;
            _smsNotification = smsNotification;
        }

        private void OnError(IdentityResult identityResult)
        {
            if (identityResult.Errors.Count() > 0)
            {
                string errorMessage = string.Empty;
                identityResult.Errors.ToList().ForEach(error => errorMessage = string.Format("{0} Code:{1},Description:{2}", errorMessage, error.Code, error.Description));
                throw new Exception(errorMessage);
            }
        }

        private ApplicationUser FindById(string userId)
        {
            return _userManager.FindByIdAsync(userId).Result;
        }

        public IList<string> GetUserRoles(string userId)
        {
            // Check for Usermanager
            if (_userManager == null)
                return null;

            var user = FindById(userId);

            if (user == null)
                throw new BusinessException("User does not exists");

            var userRoles = _userManager.GetRolesAsync(user).Result;
            return userRoles;
        }

        private void AddRoles(ApplicationUser user, IEnumerable<string> roles)
        {
            var result = _userManager.AddToRolesAsync(user, roles).Result;
        }

        private void UpdateRoles(ApplicationUser user, IEnumerable<string> roles)
        {
            var userRoles = _userManager.GetRolesAsync(user).Result;
            // Remove Existing Roles
            var result = _userManager.RemoveFromRolesAsync(user, userRoles).Result;
            AddRoles(user, roles);
        }

        private IdentityResult AddClaims(ApplicationUser applicationUser, IEnumerable<Claim> claims)
        {
            var result = _userManager.AddClaimsAsync(applicationUser, claims).Result;
            return result;
        }

        private IdentityResult UpdateClaims(ApplicationUser applicationUser, IEnumerable<Claim> claims)
        {
            var existsClaims = _userManager.GetClaimsAsync(applicationUser).Result;
            var result = _userManager.RemoveClaimsAsync(applicationUser, existsClaims).Result;
            result = AddClaims(applicationUser, claims);
            return result;
        }

        private ApplicationUser InitializeUser(UserInput userInput)
        {
            ApplicationUser applicationUser = new ApplicationUser()
            {
                Id = userInput.Id,
                UserName = userInput.UserName,
                EmailConfirmed = true,
                Email = userInput.Email,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,//userInput.TwoFactorEnabled,
                LockoutEnabled = true,//userInput.LockoutEnabled,
                AccessFailedCount = 0,//userInput.AccessFailedCount,
                PhoneNumber = userInput.PhoneNumber
            };
            return applicationUser;

        }

        /// <summary>
        /// Create Public User
        /// </summary>
        /// <returns></returns>
        public string AddPublicUser(UserInput userInput)
        {
            ApplicationUser applicationUser = InitializeUser(userInput);
            var createResult = _userManager.CreateAsync(applicationUser).Result;
            OnError(createResult);
            // Add Claim
            List<Claim> claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name , userInput.UserName),
                    new Claim(ClaimTypes.MobilePhone , userInput.PhoneNumber),
            };
            AddClaims(applicationUser, claims);
            return applicationUser.Id;

        }

        /// <summary>
        /// Create Application User
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <returns></returns>
        public async Task<string> Add(UserInput userInput)
        {
            ApplicationUser applicationUser = InitializeUser(userInput);

            // Check for Usermanager
            if (_userManager == null)
                return null;

            // If Password is Passed Update it
            if (!string.IsNullOrEmpty(applicationUser.PasswordHash))
            {
                applicationUser.PasswordHash = _userManager.PasswordHasher.HashPassword(applicationUser, applicationUser.PasswordHash);
                applicationUser.LastPasswordChangedDate = DateTime.Now;
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createResult = _userManager.CreateAsync(applicationUser).Result;
                OnError(createResult);

                var inspectionCenterId = string.Empty;
                if (!string.IsNullOrEmpty(userInput.InspectionCenterId))
                    inspectionCenterId = userInput.InspectionCenterId.ToString();

                // Add Claim
                List<Claim> claims = new List<Claim>
                {
                    new Claim(Members.InspectionCenterClaim, inspectionCenterId),
                    new Claim("email" , userInput.Email),
                    new Claim("name" , userInput.UserName),
                    new Claim("given_name" , userInput.FullName),
                    new Claim("phone_number" , userInput.PhoneNumber),
                };

                AddClaims(applicationUser, claims);

                // Add Roles
                AddRoles(applicationUser, userInput.UserRoles);
                scope.Complete();
            }

            try
            {
                var userMessage = new UserMessageModel()
                {
                    UserId = applicationUser.Id,
                    UserName = applicationUser.UserName
                };
                userMessage.EmailParams.Add(EmailPlaceholders.ReciverAddress, applicationUser.Email);
                userMessage.EmailParams.Add(EmailPlaceholders.RootSiteUrl, $"{_settings.IdentityServer.IssuerUri}{_settings.IdentityServer.ResetPasswordUrl}");
                var sendMessage = _userMessageManager.SendUserMessage(userMessage, UserMessageTemplateTypes.ForgotPassword);

                var result = _smsNotification.SendSms(new SendSmsInput()
                {
                    Number = userInput.PhoneNumber,
                    TemplateId = _settings.Notifications.Templates.SmsActivateAccount,
                    SmsParams = new Dictionary<string, string>()
                }).Result;

                if (result.result != "success")
                    throw new Exception($"Unsuccessfult attempt to send an SMS: {result.result}");

            }
            catch (Exception ex)
            {
                //_userManager.DeleteAsync(applicationUser);
                throw ex;
            }

            return applicationUser.Id;

        }

        /// <summary>
        /// Update Application User
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <returns></returns>
        public string Update(UserInput userInput)
        {
            if (_userManager == null)
                return null;
            // Get User by Username
            var user = FindById(userInput.Id);

            user.UserName = userInput.UserName;
            user.Email = userInput.Email;

            if (userInput.Status != 1 && user.LockoutEnd == null)
            {
                user.LockoutEnd = DateTime.Now.AddYears(100);
            }

            if (userInput.Status == 1 && user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = null;
            }

            //using (TransactionScope scope = new TransactionScope())
            //{
            // Update User
            var updateResult = _userManager.UpdateAsync(user).Result;

            var inspectionCenterId = string.Empty;
            if (!string.IsNullOrEmpty(userInput.InspectionCenterId))
                inspectionCenterId = userInput.InspectionCenterId.ToString();

            // Add Claim
            List<Claim> claims = new List<Claim>
                {
                    new Claim(Members.InspectionCenterClaim, inspectionCenterId),
                    new Claim("email" , userInput.Email),
                    new Claim("name" , userInput.UserName),
                    new Claim("given_name" , userInput.FullName),
                    new Claim("phone_number" , userInput.PhoneNumber),
                };

            UpdateClaims(user, claims);

            // Update Roles
            UpdateRoles(user, userInput.UserRoles);

            // Update Security Stamp
            updateResult = _userManager.UpdateSecurityStampAsync(user).Result;

            OnError(updateResult);
            //    scope.Complete();
            //}

            return user.Id;
        }
    }
}

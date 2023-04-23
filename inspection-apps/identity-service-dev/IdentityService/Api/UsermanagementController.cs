using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using IdentityService.Business;
using IdentityService.Business.Abstract;
using IdentityService.Helpers;
using IdentityService.Models;
using IdentityService.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace IdentityService.Api
{
    [Produces("application/json")]
    [Route("user-management")]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class UsermanagementController : BaseApiController
    {
        private readonly IMemberManager _memberManager;
        private readonly IAccountManager _accountManager;
        private readonly IStringLocalizer _l;
        public UsermanagementController(IMemberManager memberManager, IAccountManager accountManager, IStringLocalizer l)
        {
            _memberManager = memberManager;
            _accountManager = accountManager;
            _l = l;
        }

        [HttpGet("user-roles")]
        public IList<string> GetUserRoles(string userId)
        {
            return _memberManager.GetUserRoles(userId);
        }

        [HttpPost("add-user")]
        public async Task<ApiResultBase> AddUser([FromBody] UserInput userInput)
        {
            string userId = string.Empty;
            try
            {
                userId = await _memberManager.Add(userInput);
                return HandleResult(userId);
            }
            catch (Exception ex)
            {
                return new ManageUser() { Success = false, ErrorMessage = _l[ex.Message] };
            }
            
        }

        [HttpPost("update-user")]
        public ApiResultBase UpdateUser([FromBody] UserInput userInput)
        {
            string userId = string.Empty;
            try
            {
                userId = _memberManager.Update(userInput);
            }
            catch (Exception ex)
            {
                return new ManageUser() { Success = false, ErrorMessage = _l[ex.Message] };
            }
            return HandleResult(userId);
        }

        [HttpPost("change-password")]
        public async Task<ApiResultBase> ChangeUserPassword([FromBody] UserChangePasswordInput changePasswordInput)
        {
            ValidateModelState();
            try
            {
                await _accountManager.ChangePasswordAsync(changePasswordInput);
                return HandleResult(changePasswordInput.UserId);
            }
            catch (BusinessException ex)
            {
                return new ManageUser() {
                    Success = false,
                    ErrorMessage = _l[ex.Message],
                    Id = changePasswordInput.UserId
                };
            }
            
        }

        private ApiResultBase HandleResult(string userId)
        {
           return new ManageUser() { Success = true, Id = userId };            
        }

    }
}
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

namespace IdentityService.Api
{
    [Produces("application/json")]
    [Route("administration")]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class AdministrationController : BaseApiController
    {
        private readonly IMemberManager _memberManager;
        private readonly IAccountManager _accountManager;

        public AdministrationController(IMemberManager memberManager, IAccountManager accountManager)
        {
            _memberManager = memberManager;
            _accountManager = accountManager;
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
                return new ManageUser() { Success = false, ErrorMessage = ex.Message };
            }
            
        }

       

        private ApiResultBase HandleResult(string userId)
        {
           return new ManageUser() { Success = true, Id = userId };            
        }

    }
}
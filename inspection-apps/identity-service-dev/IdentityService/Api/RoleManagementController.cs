using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityService.Business.Abstract;
using IdentityService.Helpers;
using IdentityService.Models;
using IdentityService.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api
{
    [Produces("application/json")]
    [Route("role-management")]
    public class RoleManagementController : BaseApiController
    {
        private readonly RoleManager<IdentityRole> _roleManager;       
        public RoleManagementController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;            
        }

        [HttpGet("roles")]
        public List<RolesOutput> GetRoles()
        {
            List<RolesOutput> roleOutput = new List<RolesOutput>();
            var roles = _roleManager.Roles;
            roles.ToList().ForEach(roleItem => roleOutput.Add(new RolesOutput { Id = roleItem.Id, Name = roleItem.Name }));
            return roleOutput;
        }
    }
}
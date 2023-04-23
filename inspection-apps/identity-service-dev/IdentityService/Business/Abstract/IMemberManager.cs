using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Models;
using IdentityService.Models.Dto;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Business.Abstract
{
    public interface IMemberManager
    {
        Task<string> Add(UserInput userInput);
        string AddPublicUser(UserInput userInput);
        string Update(UserInput userInput);
        IList<string> GetUserRoles(string userId);       
    }
}

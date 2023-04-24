using System.Threading.Tasks;
using App.IdentityServer.Helpers;
using App.IdentityServer.Models.Dto;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.IdentityServer.Api;

[Produces("application/json")]
[Route("authentication")]
public class AuthenticationController: BaseApiController
{
    private readonly TestUserStore _users;
    private readonly IEventService _events;
    
    public AuthenticationController(TestUserStore users, IEventService events)
    {
        _users = users;
        _events = events;
    }
    [HttpPost("sign-in")]
    public async Task<ApiResultBase> SignIn([FromBody] SignInInfoInput model)
    {
        var user = _users.FindByUsername(model.LoginName);
        
        await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username));
        
        var isuser = new IdentityServerUser(user.SubjectId)
        {
            DisplayName = user.Username
        };
        
        AuthenticationProperties props = null;
        await HttpContext.SignInAsync(isuser, props);
        
        return new SignInOutput() { Success = true };
    }
}
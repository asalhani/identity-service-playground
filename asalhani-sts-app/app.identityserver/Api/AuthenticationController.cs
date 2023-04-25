using System.Threading.Tasks;
using App.IdentityServer.Helpers;
using App.IdentityServer.Models.Dto;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.IdentityServer.Api;

[Produces("application/json")]
[Route("authentication")]
public class AuthenticationController : BaseApiController
{
    private readonly TestUserStore _users;
    private readonly IEventService _events;
    private readonly IIdentityServerInteractionService _interaction;

    public AuthenticationController(TestUserStore users, IEventService events,
        IIdentityServerInteractionService interaction)
    {
        _users = users;
        _events = events;
        _interaction = interaction;
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

        return new SignInOutput() {Success = true};
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
        result.ClientName = string.IsNullOrEmpty(logoutContext?.ClientName)
            ? logoutContext?.ClientId
            : logoutContext?.ClientName;
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
        var result = new SignOutOutput() {IsShowPrompt = false};
        //**************************************************************************
        // get context information (client name, post logout redirect URI and iframe for federated signout)
        var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);
        result.ClientName = string.IsNullOrEmpty(logoutContext?.ClientName)
            ? logoutContext?.ClientId
            : logoutContext?.ClientName;
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

            // TODO: asalhani: enable when ApplicationUser activicated with Identity Svc
            // var user = await _userManager.GetUserAsync(User);
            // await _userManager.UpdateSecurityStampAsync(user);
            // await _signInManager.SignOutAsync();

            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
        }

        return result;
    }
}
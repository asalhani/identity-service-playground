using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;

namespace App.IdentityServer.Business;

 public class UserProfileService : IProfileService
    {
        private UserManager<ApplicationUser> _userManager;
        private IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly IResourceStore _resources;


        public UserProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            IResourceStore resourceStore)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _resources = resourceStore;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // var sub = context.Subject.GetSubjectId();
            // var user = await _userManager.FindByIdAsync(sub);
            // var principal = await _claimsFactory.CreateAsync(user);
            //
            // // add all allowed client API
            // var allowed_apis = await _resources.FindApiResourcesByScopeNameAsync(context.Client.AllowedScopes);
            // context.RequestedResources.ApiResources = allowed_apis.Distinct().ToList();
            //
            // // get all required API and identity claims
            // var required_api_claims = allowed_apis.SelectMany(a => a.UserClaims)
            //     .Union(context.RequestedResources.IdentityResources.SelectMany(r => r.UserClaims))
            //     .Distinct();
            //
            // var claims = principal.Claims.ToList();
            //
            // claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type) ||
            //     required_api_claims.Contains(claim.Type)).ToList();
            // if (!claims.Any(c => c.Type == JwtClaimTypes.GivenName))
            // {
            //     claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));
            // }
            //
            // // add user roles as claims
            // var roles = await _userManager.GetRolesAsync(user);
            // foreach (var role in roles)
            // {
            //     claims.Add(new Claim(JwtClaimTypes.Role, role));
            // }
            // var orginal_client = context.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.ClientId)?.Value;
            // if (orginal_client != null && orginal_client != context.Client.ClientId)
            // {
            //     //add original client id to claims in case of delegation:
            //     claims.Add(new Claim("original_client_id", orginal_client));
            // }
            //
            // // remove duplications in claims if any:
            // var distinct_claims = claims.GroupBy(x => new { x.Type, x.Value }).Select(g => g.First()).ToList();
            //
            // context.IssuedClaims = distinct_claims;

        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
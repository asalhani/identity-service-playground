using System.Threading.Tasks;
using App.IdentityServer.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace App.IdentityServer
{
    /// <summary>
    /// To support the role-based access control in our IDP
    /// </summary>
    public class CustomProfileService : IProfileService
    {
        /// <summary>
        /// logic to include required claims for a user using the context object
        /// </summary>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = InMemoryConfig.GetUsers()
                .Find(u => u.SubjectId.Equals(sub));
            context.IssuedClaims.AddRange(user.Claims);
            return Task.CompletedTask;
        }

        /// <summary>
        /// determine if a user is currently allowed to obtain tokens
        /// </summary>
        public Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = InMemoryConfig.GetUsers()
                .Find(u => u.SubjectId.Equals(sub));
            context.IsActive = user != null;
            return Task.CompletedTask;
        }
    }
}
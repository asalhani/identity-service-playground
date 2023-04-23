using IdentityServer4;
using IdentityService.Models.Configurations;
using Microsoft.Extensions.Options;
using PlatformCommons.Service.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityService.Helpers
{
    public class ISAuthorizationTokenProvider : IAuthorizationTokenProvider
    {
        private readonly IdentityServerTools _identityServerTools;
        private readonly AppSettings _settings;

        public ISAuthorizationTokenProvider(IdentityServerTools identityServerTools, IOptions<AppSettings> settings)
        {
            _identityServerTools = identityServerTools;
            _settings = settings.Value;
        }

        private string _token = null;
        public string Token
        {
            get
            {
                if (_token == null)
                {
                    _token = "Bearer " + _identityServerTools.IssueClientJwtAsync(
                        "inspection_platform_identity_server",
                        120,
                        _settings.Notifications.NotificationServiceScopes,
                        _settings.Notifications.NotificationServiceScopes).Result;
                }
                return _token;
            }
        }

        public void InsureToken(string clientId, string clientSecret, string scopes)
        {
            throw new NotImplementedException();
        }
    }
}

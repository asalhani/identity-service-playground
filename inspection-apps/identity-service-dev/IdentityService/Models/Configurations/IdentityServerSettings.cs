using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Configurations
{
    public class IdentityServerSettings
    {
        public string ApiName { get; set; }
        public string IssuerUri { get; set; }
        public string PublicOrigin { get; set; }
        public string LoginPageUrl { get; set; }
        public string LogoutPageUrl { get; set; }
        public string ResetPasswordUrl { get; set; }
        public string SigninKeyCredentials { get; set; }
        public string CertificateFilePath { get; set; }
        public string CertificatePassword { get; set; }
        public string CookiePath { get; set; }
        public int AccessTokenLifetimeInSeconds { get; set; }
    }
}

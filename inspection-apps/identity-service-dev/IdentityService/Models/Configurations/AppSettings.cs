using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Configurations
{
    public class AppSettings
    {
        public IdentityServerSettings IdentityServer { get; set; }
        public CaptchaSettings Captcha { get; set; }
        public UserAccountSettings UserAccount { get; set; }
        public PasswordPolicySettings PasswordPolicy { get; set; }
        public NotificationsSettings Notifications { get; set; }
        public ClientDatabaseConfig ClientDatabaseConfig { get; set; }
        public string IdentityServiceURL { get; set; }
        public string ApiName { get; set; }
        public DataProtectionSettings DataProtection  { get; set; }
        public ServiceClientsSettings ClientsSettings { get; set; }
    }
}

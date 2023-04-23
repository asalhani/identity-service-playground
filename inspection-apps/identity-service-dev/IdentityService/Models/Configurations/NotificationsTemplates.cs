using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Configurations
{
    public class NotificationsTemplates
    {
        public int SmsSignInOtp { get; set; }
        public int SmsResetPasswordOtp { get; set; }
        public int SmsActivateAccount { get; set; }
        public int EmailResetPassword { get; set; }
    }
}

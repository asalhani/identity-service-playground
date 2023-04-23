using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Configurations
{
    public class UserAccountSettings
    {
        public bool IsOtpEnabled { get; set; }
        public int OtpAttemptsTimeoutInMinutes { get; set; }
        public int OtpAllowResendAfterSeconds { get; set; }
        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }
        public int LockoutTimeSpanInMinutes { get; set; }
        public int UserMessageTokenExpirationInMinutes { get; set; }
        public int MaxPasswordAgeInDays { get; set; }

    }
}

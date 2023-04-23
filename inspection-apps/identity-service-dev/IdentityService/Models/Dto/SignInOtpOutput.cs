using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models
{
    public class SignInOtpOutput : OtpOutput
    {
        public bool IsAccountLocked { get; set; } = false;
        public bool IsPasswordExpired { get; set; } = false;
        public string ChangeExpiredPasswordToken { get; set; } = null;
    }
}

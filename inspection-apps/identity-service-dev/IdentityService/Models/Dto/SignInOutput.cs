using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class SignInOutput : RequireOtpOuputBase
    {
        public bool IsOtpRequired { get; set; } = false;
        public bool IsInvalidCaptcha { get; set; } = false;
        public bool IsInvalidUserInfo { get; set; } = false;
        public bool IsAccountLocked { get; set; } = false;
        public bool IsPasswordExpired { get; set; } = false;
        public string ChangeExpiredPasswordToken { get; set; } = null;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class ResetPasswordOuput : RequireOtpOuputBase
    {
        public bool IsInvalidCaptcha { get; set; } = false;
    }
}

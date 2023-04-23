using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models
{
    public class OtpOutput : ApiResultBase
    {
        public bool IsInvalidOtp { get; set; } = false;
        public bool IsOtpExpired { get; set; }
    }
}

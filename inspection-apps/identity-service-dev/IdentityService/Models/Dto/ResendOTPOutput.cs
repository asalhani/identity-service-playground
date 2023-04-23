using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class ResendOTPOutput : ApiResultBase
    {
        public bool IsSessionExpired { get; set; } = false;
        public string Error { get; set; }

    }
}

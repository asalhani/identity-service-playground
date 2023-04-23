using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class ForgotPasswordOutput : ApiResultBase
    {
        //just a dummy field used in case of debugging to hold the activation url
        public string Info { get; set; }
        public bool IsInvalidCaptcha { get; set; } = false;
    }
}

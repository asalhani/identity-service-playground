using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public abstract class RequireOtpOuputBase : ApiResultBase
    {
        public string MobileNumberFraction { get; set; }
        public int AttemptsTimeoutInMintutes { get; set; }
        public string Issues { get; set; }
    }
}

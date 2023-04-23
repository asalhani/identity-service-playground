using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class SignOutOutput : ApiResultBase
    {
        public new bool Success { get; } = true;
        public bool IsShowPrompt { get; set; } = true;

        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
    }
}

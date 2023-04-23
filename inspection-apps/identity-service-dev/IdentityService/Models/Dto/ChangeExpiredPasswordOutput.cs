using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class ChangeExpiredPasswordOutput : ApiResultBase
    {
        public bool IsInvalidRequest { get; set; } = false;
        public bool IsInvalidNewPassword { get; set; } = false;
        public bool IsInvalidCaptcha { get; set; } = false;
        public string InvalidPasswordIssues { get; set; } = null;
    }
}

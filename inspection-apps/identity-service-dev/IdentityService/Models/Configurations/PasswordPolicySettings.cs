using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Configurations
{
    public class PasswordPolicySettings
    {
        public int MinPasswordLength { get; set; }
        public int MaxConsecutiveLetters { get; set; }
    }
}

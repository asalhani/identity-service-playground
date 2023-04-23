using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class ForgotPasswordInput
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string CaptchaString { get; set; }
    }
}

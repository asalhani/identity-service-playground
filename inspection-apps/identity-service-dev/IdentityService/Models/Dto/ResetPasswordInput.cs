using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class ResetPasswordInput
    {
        [Required]
        public string EmailToken { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [MaxLength(1900)]
        public string CaptchaString { get; set; }
    }
}

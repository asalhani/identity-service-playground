using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class ChangeExpiredPasswordInput
    {
        [Required]
        [EmailAddress]
        public string LoginName { get; set; }
        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ChangePasswordToken { get; set; }

        [Required]
        [MaxLength(1900)]
        public string CaptchaString { get; set; }
    }
}

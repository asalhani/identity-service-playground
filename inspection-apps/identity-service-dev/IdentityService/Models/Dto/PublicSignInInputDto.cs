using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class PublicSignInInputDto
    {
        [Required]
        public string MobileNumber { get; set; }
    }
}

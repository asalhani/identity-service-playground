using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class OtpInput
    {
        [Required]
        //  [RegularExpression(@"^[0-9]*$")]
        public string OtpValue { get; set; }

    }
}

﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime? LastPasswordChangedDate { get; set; }
    }
}

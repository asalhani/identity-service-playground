using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models
{
    public abstract class ApiResultBase
    {
        public bool Success { get; set; }
    }
}

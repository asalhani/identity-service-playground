using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class ManageUser : ApiResultBase
    {
        public string Id { get; set; }
        public string ErrorMessage { get; set; }
    }
}

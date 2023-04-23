using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Dto
{
    public class UserInput
    {
        public virtual string Id { get; set; }
         public virtual string FullName { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public string InspectionCenterId { get; set; }
        public List<string> UserRoles { get; set; }
        public string PhoneNumber { get; set; }
        public int Status { get; set; }
    }
}

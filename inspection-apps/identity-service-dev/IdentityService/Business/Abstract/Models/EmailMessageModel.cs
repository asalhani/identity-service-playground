using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Business.Abstract.Models
{
    public class EmailMessageModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string InvitationToken { get; set; }
        public bool IsUsed { get; set; } = false;
        public bool IsCancelled { get; set; }
        public int EmailtemplateTypeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Dictionary<string, string> EmailParams { get; set; } = new Dictionary<string, string>();
    }
}

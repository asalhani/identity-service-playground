using Dapper;
using IdentityService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.DataAccess.Abstract.Models
{
    [Table("UserMessage" , Schema ="IdentityService")]
    public class UserMessageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }

        //[NotMapped]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string InvitationToken { get; set; }
        public bool IsUsed { get; set; } = false;
        public bool IsCancelled { get; set; }
        public int UserMessageTypeId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [NotMapped]
        public Dictionary<string, string> EmailParams { get; set; } = new Dictionary<string, string>();



    }
}

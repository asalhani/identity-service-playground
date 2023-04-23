using IdentityService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.DataAccess.Abstract.Models
{
    [Table("AspNetPasswordHistory", Schema = "IdentityService")]
    public class AspNetPasswordHistoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AspNetPasswordHistoryId { get; set; }

        //[NotMapped]
        public string UserId { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public string Password { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}

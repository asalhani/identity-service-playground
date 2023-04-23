using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IdentityService.ServiceDeployment.Dto
{
    public class ServiceIdentityDetails
    {
        [Required]
        public string ApiCode { get; set; }
        public string ApiDisplayName { get; set; }
        public string ApiDescription { get; set; }
        public IEnumerable<string> ApiUserClaims { get; set; } 
        [Required]
        public IEnumerable<string> Scopes { get; set; }
        public IEnumerable<ServiceClientDetails> ClientDetails { get; set; }
        public IEnumerable<string> ApiRoles { get; set; }
    }
}

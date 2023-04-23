using System;
using System.Collections.Generic;
using System.Linq;
namespace IdentityService.ServiceDeployment.Dto
{
    public class ServiceClientDetails
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }    
        public string GrantType { get; set; }
        public string ClientAllowedScopes { get; set; }
        public IDictionary<string, string> ClientClaims { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IdentityService.ServiceDeployment.Dto
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ServiceDeploymentResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public IDictionary<string, string> Summary { get; set; }
    }
}

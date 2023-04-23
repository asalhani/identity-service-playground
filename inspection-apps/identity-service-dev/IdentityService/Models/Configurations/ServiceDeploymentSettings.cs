using System.Collections.Generic;

namespace IdentityService.Models.Configurations
{
    public class ServiceClientsSettings
    {
        public IEnumerable<InternalClientSettings> InternalClients { get; set; }

    }

    public class InternalClientSettings
    {
        public string ClientId { get; set; }
        public string ClientScopeSuffix { get; set; }
        public string ClientSecret { get; set; }
    }
}
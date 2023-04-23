using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Configurations
{
    public class ClientDatabaseConfig
    {
        public string ClientUrl { get; set; }
        public string ClientNameId { get; set; }
        public string ClientName { get; set; }
        public string AdminEmail { get; set; }
        public string AdminMobile { get; set; }
        public string AdminName { get; set; }
    }
}

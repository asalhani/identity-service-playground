using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Configurations
{
    public class NotificationsSettings
    {
        public string NotificationServiceUrl { get; set; }
        public IEnumerable<string> NotificationServiceScopes { get; set; }
        public NotificationsTemplates Templates { get; set; }

    }
}

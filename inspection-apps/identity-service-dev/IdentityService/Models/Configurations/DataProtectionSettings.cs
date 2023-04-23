using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Configurations
{
    public class DataProtectionSettings
    {
        public string CertificateIssuerName { get; set; }
        public string KeyFolderName { get; set; }
        public bool IsRequired { get; set; }
        public string CertificateFilePath { get; set; }
        public string CertificatePassword { get; set; }
    }
}

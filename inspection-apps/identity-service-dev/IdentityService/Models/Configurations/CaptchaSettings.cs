using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Configurations
{
    public class CaptchaSettings
    {
        public bool EnableCaptchaValidation { get; set; }
        public string ProxyServer { get; set; }
        public int ProxyServerPort { get; set; }
        public string ReCaptchaPrivateKey { get; set; }
        public string ReCaptchaValidateUrl { get; set; }
        public string CaptchaType { get; set; }
        public string CustomCaptchaCharacters { get; set; }
        public int CustomCaptchaWidth { get; set; }
        public int CustomCaptchaHeight { get; set; }
        public string HashSalt { get; set; }
        public int CaptchaCharacterLength { get; set; }
        public string FontFamilyName { get; set; }
    }
}

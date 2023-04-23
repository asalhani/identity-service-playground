using IdentityService.Helpers.Abstract;
using IdentityService.Models.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace IdentityService.Helpers
{
    public class RecaptchaValidator : ICaptchaValidator
    {
        private readonly AppSettings _settings;
        public RecaptchaValidator(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public bool Validate(string EncodedResponse)
        {            
            var validateCaptcha = _settings.Captcha.EnableCaptchaValidation;
            if (!validateCaptcha)
                return true;
            string proxyServer = _settings.Captcha.ProxyServer; 
            int proxyServerPort = _settings.Captcha.ProxyServerPort;
            string privateKey = _settings.Captcha.ReCaptchaPrivateKey; 
            string googleUrl = _settings.Captcha.ReCaptchaValidateUrl;
            RecaptchaValidator result;
            using (var client = new WebClient())
            {
                if (!string.IsNullOrEmpty(proxyServer))
                    client.Proxy = new WebProxy(proxyServer, proxyServerPort); 
                var googleReply = client.DownloadString(string.Format(googleUrl, privateKey, EncodedResponse));
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<RecaptchaValidator>(googleReply);
            }
            return bool.Parse(result.Success);
        }

        [JsonProperty("success")]
        public string Success
        {
            get; set;
        }
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get;
            set;
        }
    }
}

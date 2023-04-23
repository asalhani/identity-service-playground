using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Helpers.Abstract;
using IdentityService.Models.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace IdentityService.Helpers
{
    public class CustomCaptchaValidator : ICaptchaValidator
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly AppSettings _settings;
        private readonly IDistributedCache _distributedCache;

        public CustomCaptchaValidator(IHttpContextAccessor httpContext, IOptions<AppSettings> settings, IDistributedCache distributedCache)
        {
            _httpContext = httpContext;
            _settings = settings.Value;
            _distributedCache = distributedCache;
        }
        public bool Validate(string captchaCode)
        {
            var validateCaptcha = _settings.Captcha.EnableCaptchaValidation;
            if (!validateCaptcha)
                return true;

            var codeKey = _httpContext.HttpContext.Request.Cookies?[Captcha.CaptchaCode];

            if (string.IsNullOrWhiteSpace(codeKey) || string.IsNullOrWhiteSpace(captchaCode))
                return false;


            var storedCode = _distributedCache.GetString(codeKey);

            //generated captcha always expire after one validation attempt
            _httpContext.HttpContext.Response.Cookies.Delete(Captcha.CaptchaCode);
            _distributedCache.Remove(codeKey);

            return captchaCode.Equals(storedCode);
        }
    }
}

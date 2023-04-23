using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityService.Helpers;
using IdentityService.Models.Configurations;
using IdentityService.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace IdentityService.Api
{
    [Produces("application/json")]
    [Route("captcha")]
    public class CaptchaController : BaseApiController
    {
        private const int MaxCaptchaLifespanInSeconds = 300;

        private readonly AppSettings _settings;
        private readonly IDistributedCache _distributedCache;

        private readonly DistributedCacheEntryOptions captchaCacheOptions;

        public CaptchaController(IOptions<AppSettings> settings, IDistributedCache distributedCache)
        {
            _settings = settings.Value;
            _distributedCache = distributedCache;
            captchaCacheOptions = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(MaxCaptchaLifespanInSeconds));

        }

        [HttpGet]
        public IActionResult GetCaptchaImage()
        {
            var captchaCode = Captcha.GenerateCaptchaCode(_settings.Captcha.CustomCaptchaCharacters, _settings.Captcha.CaptchaCharacterLength);
            var result = Captcha.GenerateCaptchaImage(_settings.Captcha.CustomCaptchaWidth, _settings.Captcha.CustomCaptchaHeight, captchaCode, _settings.Captcha.FontFamilyName);
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddSeconds(MaxCaptchaLifespanInSeconds);
            //option.Secure = true;

            //generate secure random key
            var captchaToken = Captcha.GetRandomSecureKey();

            //store token temporary
            _distributedCache.SetString(captchaToken, captchaCode, captchaCacheOptions);

            HttpContext.Response.Cookies.Append("captchacode", captchaToken, option);

            Stream captchaStream = new MemoryStream(result.CaptchaByteData);
            var captchaResult = new FileStreamResult(captchaStream, "image/png");
            return captchaResult;
        }
    }
}

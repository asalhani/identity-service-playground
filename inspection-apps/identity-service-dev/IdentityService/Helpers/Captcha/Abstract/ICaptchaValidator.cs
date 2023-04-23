using IdentityService.Models.Configurations;
using Microsoft.AspNetCore.Http;

namespace IdentityService.Helpers.Abstract
{
    public interface ICaptchaValidator
    {
        bool Validate(string captchaCode);
    }
}
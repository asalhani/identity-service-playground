using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IdentityService.Api
{
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        public HomeController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        public IActionResult Index()
        {
            return Redirect("~/index.html");
        }

        /// <summary>
        /// Display IdentityServer errors as structured json response
        /// </summary>
        /// <param name="errorId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Error(string errorId)
        {
            var message = await _interaction.GetErrorContextAsync(errorId);
            
            if (message == null)
            {
                message = new ErrorMessage()
                {
                    Error = "invalid_request",
                    ErrorDescription = "Invalid Request",
                    RequestId = HttpContext.TraceIdentifier
                };
            }

            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(message,
                    new JsonSerializerSettings
                    { NullValueHandling = NullValueHandling.Ignore }),
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
    }
}
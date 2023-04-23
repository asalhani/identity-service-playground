using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Helpers
{
    public class BaseApiController : Controller
    {
        protected void ValidateModelState()
        {
            if (!ModelState.IsValid)
            {
                //var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                //throw new MalformedInputException("Invalid ModelState for the request: " + errors);
                string errors = "";
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errors += error.ErrorMessage;
                        if (error.Exception != null)
                        {
                            errors += "(" + error.Exception.Message + ") ";
                        }
                        errors += ". ";
                    }
                }
                throw new InvalidDataException("Invalid ModelState for the request: " + errors);
            }
        }
    }
}

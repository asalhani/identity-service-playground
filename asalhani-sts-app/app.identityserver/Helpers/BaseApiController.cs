using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace App.IdentityServer.Helpers;

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
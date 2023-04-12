using Microsoft.AspNetCore.Mvc;

namespace IdentityServerApp;

[ApiController]
[Route("[controller]")]
public class Home : ControllerBase
{
    // GET
    [HttpGet()]
    public IActionResult Index()
    {
        return Ok("ok response");
    }
}
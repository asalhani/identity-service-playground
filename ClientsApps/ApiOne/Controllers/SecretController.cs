using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOne.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class SecretController : Controller
{
    // GET
    [HttpGet("secretapione")]
    [Authorize]
    public IActionResult Index()
    {
        return Ok("Secret message from ApiOne");
    }
}
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace ApiTwo.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string IdentityServerUrl = "https://localhost:7220";
    private const string ApiOneBaseUrl = "https://localhost:7003/api";

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET
    [HttpGet("callapione")]
    public async Task<IActionResult> Index()
    {
        var serverClient = _httpClientFactory.CreateClient();
        var discoveryDoc = await serverClient.GetDiscoveryDocumentAsync(IdentityServerUrl);
        var tokenResponse =  await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
        {
            Address = discoveryDoc.TokenEndpoint,
            ClientId = "client_id",
            ClientSecret = "client_secret",
            Scope = "ApiOne",
        });
        
        // call ApiOne secret endpoint
        var apiOneClient = _httpClientFactory.CreateClient();
        apiOneClient.SetBearerToken(tokenResponse.AccessToken);
        var apiOneSecretResponse = await apiOneClient.GetAsync($"{ApiOneBaseUrl}/Secret/secretapione");
        var responseContent = await apiOneSecretResponse.Content.ReadAsStringAsync();
        return Ok(new { access_token = tokenResponse.AccessToken, message = responseContent });
    }
}
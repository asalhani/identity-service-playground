namespace App.IdentityServer.Models.Dto;

public class SignOutOutput : ApiResultBase
{
    public new bool Success { get; } = true;
    public bool IsShowPrompt { get; set; } = true;

    public string PostLogoutRedirectUri { get; set; }
    public string ClientName { get; set; }
}
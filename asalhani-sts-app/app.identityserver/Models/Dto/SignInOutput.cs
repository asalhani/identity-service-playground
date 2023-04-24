namespace App.IdentityServer.Models.Dto;

public class SignInOutput : RequireOtpOuputBase
{
    public bool IsAccountLocked { get; set; } = false;
}
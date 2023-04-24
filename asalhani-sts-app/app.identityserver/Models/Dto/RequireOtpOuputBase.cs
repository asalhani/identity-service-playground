namespace App.IdentityServer.Models.Dto;

public abstract class RequireOtpOuputBase : ApiResultBase
{
    public string Issues { get; set; }
}
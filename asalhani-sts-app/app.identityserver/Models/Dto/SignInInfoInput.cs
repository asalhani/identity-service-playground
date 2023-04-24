using System.ComponentModel.DataAnnotations;

namespace App.IdentityServer.Models.Dto;

public class SignInInfoInput
{
    [Required]
    public string LoginName { get; set; }
}
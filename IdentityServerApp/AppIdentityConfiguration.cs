using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServerApp;

public static class AppIdentityConfiguration
{
    public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4",
                Username = "Mick",
                Password = "MickPassword",
                Claims = new List<Claim>
                {
                    new Claim("given_name", "Mick"),
                    new Claim("family_name", "Mining")
                }
            },
            new TestUser
            {
                SubjectId = "123",
                Username = "Gowtham",
                Password = "Test@123",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Gowtham K"),
                    new Claim(JwtClaimTypes.GivenName, "Gowtham"),
                    new Claim(JwtClaimTypes.FamilyName, "Kumar"),
                    new Claim(JwtClaimTypes.WebSite, "https://gowthamcbe.com/"),
                }
            }
        };
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("api.read"),
            new ApiScope("api.write"),
            new ApiScope("companyApi", "CompanyEmployee API")
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("ApiOne")
            {
                Scopes = new List<string>{ "api.read", "api.write" },
                ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) }
                
            },
            new ApiResource("ApiThree")
            {
                Scopes = new List<string>{ "api.read", "api.write" },
                ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) }
                
            },
            new ApiResource("companyApi", "CompanyEmployee API") 
            { 
                Scopes = { "companyApi", IdentityServerConstants.StandardScopes.OpenId } 
            } 
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "svc_three_client",
                ClientName = "Client Credentials Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "api.read" }
            },
            new Client
            {
                ClientId = "svc_two_client",
                ClientName = "Client Credentials Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "api.read" }
            },
            new Client
            {
                ClientId = "company-employee",
                ClientSecrets = new [] { new Secret("codemazesecret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "companyApi" }
            }
        };

}
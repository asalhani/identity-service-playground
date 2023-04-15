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
            },
            new TestUser
            {
                SubjectId = "818727", Username = "alice", Password = "alice",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address,
                        @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }
            },
            new TestUser
            {
                SubjectId = "88421113", Username = "bob", Password = "bob",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim(JwtClaimTypes.Address,
                        @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
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
                Scopes = new List<string> {"api.read", "api.write"},
                ApiSecrets = new List<Secret> {new Secret("supersecret".Sha256())}
            },
            new ApiResource("ApiThree")
            {
                Scopes = new List<string> {"api.read", "api.write"},
                ApiSecrets = new List<Secret> {new Secret("supersecret".Sha256())}
            },
            new ApiResource("companyApi", "CompanyEmployee API")
            {
                Scopes = {"companyApi", IdentityServerConstants.StandardScopes.OpenId}
            },
            new ApiResource("resourceApi", "API Application")
        };

    private static string spaClientUrl = "https://localhost:44311";

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "svc_three_client",
                ClientName = "Client Credentials Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedScopes = {"api.read"}
            },
            new Client
            {
                ClientId = "svc_two_client",
                ClientName = "Client Credentials Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedScopes = {"api.read"}
            },
            new Client
            {
                ClientId = "company-employee",
                ClientSecrets = new[] {new Secret("codemazesecret".Sha256())},
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, "companyApi"}
            },
            new Client
            {
                ClientId = "clientApp",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = {"resourceApi"}
            },

            // OpenID Connect implicit flow client (MVC)
            new Client
            {
                ClientId = "mvc",
                ClientName = "MVC Client",
                AllowedGrantTypes = GrantTypes.Hybrid,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                RedirectUris = {$"{spaClientUrl}/signin-oidc"},
                PostLogoutRedirectUris = {$"{spaClientUrl}/signout-callback-oidc"},
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "resourceApi"
                },
                AllowOfflineAccess = true
            },
            new Client
            {
                ClientId = "spaClient",
                ClientName = "SPA Client",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = {$"{spaClientUrl}/callback"},
                PostLogoutRedirectUris = {$"{spaClientUrl}/"},
                AllowedCorsOrigins = {$"{spaClientUrl}"},
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "resourceApi"
                }
            },
            new Client
            {
                ClientId = "spaCodeClient",
                ClientName = "SPA Code Client",
                AccessTokenType = AccessTokenType.Jwt,
                // RequireConsent = false,
                AccessTokenLifetime = 330, // 330 seconds, default 60 minutes
                IdentityTokenLifetime = 30,

                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,

                AllowAccessTokensViaBrowser = true,
                RedirectUris = new List<string>
                {
                    $"{spaClientUrl}/callback",
                    $"{spaClientUrl}/silent-renew.html",
                    "https://localhost:4200",
                    "https://localhost:4200/silent-renew.html"
                },
                PostLogoutRedirectUris = new List<string>
                {
                    $"{spaClientUrl}/unauthorized",
                    $"{spaClientUrl}",
                    "https://localhost:4200/unauthorized",
                    "https://localhost:4200"
                },
                AllowedCorsOrigins = new List<string>
                {
                    $"{spaClientUrl}",
                    "https://localhost:4200"
                },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "resourceApi"
                }
            },
        };
}
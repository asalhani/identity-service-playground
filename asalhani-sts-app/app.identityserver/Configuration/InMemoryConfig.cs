using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace App.IdentityServer.Configuration
{
    public static class InMemoryConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
          new List<IdentityResource>
          {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
              new IdentityResources.Address(),
              new IdentityResource("roles", "User role(s)", new List<string> { "role" }),
              new IdentityResource("position", "Your position", new List<string> { "position" }), 
              new IdentityResource("country", "Your country", new List<string> { "country" })
          };

        public static IEnumerable<ApiScope> GetApiScopes() =>
           new List<ApiScope> { new ApiScope("companyApi", "CompanyEmployee API") };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("companyApi", "CompanyEmployee API")
                {
                    Scopes = { "companyApi" }
                }
            };

        public static List<TestUser> GetUsers() =>
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
                      new Claim("family_name", "Mining"),
                      new Claim("address", "Sunny Street 4"),
                      new Claim("role", "Admin"),
                      new Claim("position", "Administrator"), 
                      new Claim("country", "USA")
                  }
              },
              new TestUser
              {
                  SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340",
                  Username = "Jane",
                  Password = "JanePassword",
                  Claims = new List<Claim>
                  {
                      new Claim("given_name", "Jane"),
                      new Claim("family_name", "Downing"),
                      new Claim("address", "Long Avenue 289"),
                      new Claim("role", "Visitor"),
                      new Claim("position", "Viewer"),
                      new Claim("country", "USA")
                  }
              },
              new TestUser
              {
                  SubjectId = "cc2559c9-8623-407f-a4d2-3f440e6ea47e",
                  Username = "2088755802",
                  Password = "2088755802",
                  Claims = new List<Claim>
                  {
                      new Claim("given_name", "adib"),
                      new Claim("family_name", "salhani"),
                      new Claim("address", "sulimanyah, riyadh 123"),
                      new Claim("role", "Admin"),
                      new Claim("position", "Administrator"),
                      new Claim("country", "KSA")
                  }
              },
          };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
               new Client
               {
                    ClientId = "company-employee",
                    ClientSecrets = new [] { new Secret("codemazesecret".Sha512()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "companyApi" }
               },
               new Client
               {
                   ClientName = "MVC Client",
                   ClientId = "mvc-client",
                   AllowedGrantTypes = GrantTypes.Hybrid,
                   RedirectUris = new List<string>{ "https://localhost:5010/signin-oidc" },
                   RequirePkce = false,
                   AllowedScopes = 
                   { 
                       IdentityServerConstants.StandardScopes.OpenId, 
                       IdentityServerConstants.StandardScopes.Profile,
                       IdentityServerConstants.StandardScopes.Address,
                       "roles",
                       "companyApi",
                       "position",
                       "country"
                   },
                   ClientSecrets = { new Secret("MVCSecret".Sha512()) },
                   PostLogoutRedirectUris = new List<string> { "https://localhost:5010/signout-callback-oidc" },
                   RequireConsent = true
               },
               
               // angular oidc client
               new Client
               {
                   ClientName = "Angular-Client",
                   ClientId = "angular-client",
                   AllowedGrantTypes = GrantTypes.Code, // -- CHECK
                   RedirectUris = new List<string>{ "http://localhost:4200/signin-callback", "http://localhost:4200/assets/silent-callback.html" },
                   RequirePkce = true, // -- CHECK
                   AllowAccessTokensViaBrowser = true,
                   AllowedScopes =
                   {
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile,
                       "companyApi"
                   },
                   AllowedCorsOrigins = { "http://localhost:4200" },
                   RequireClientSecret = false,
                   PostLogoutRedirectUris = new List<string> { "http://localhost:4200/signout-callback" },
                   RequireConsent = false,
                   AccessTokenLifetime = 600
               },
               new Client
               {
                   ClientName = "Angular SPA Client",
                   ClientId = "angular-spa-client",
                   AllowedGrantTypes = GrantTypes.Code, // -- CHECK
                   RedirectUris = new List<string>{ "http://localhost:4200/#/identity-guards/auth-callback#", "http://localhost:4200/#/identity-guards/silent-refresh#" },
                   RequirePkce = true, // -- CHECK
                   AllowAccessTokensViaBrowser = true,
                   AllowedScopes =
                   {
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile,
                       "companyApi"
                   },
                   AllowedCorsOrigins = { "http://localhost:4200" },
                   RequireClientSecret = false,
                   PostLogoutRedirectUris = new List<string> { "http://localhost:4200/signout-callback", "http://localhost:4200" },
                   RequireConsent = false,
                   AccessTokenLifetime = 600
               }
            };
    }
}

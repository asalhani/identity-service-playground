using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityService.ServiceDeployment.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IdentityServer4;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IdentityService.Models.Configurations;
using Microsoft.Extensions.Options;

namespace IdentityService.ServiceDeployment
{
    [Produces("application/json")]
    [Route("service-identity-deployment")]
    [ApiController]
    
    public class ServiceDeploymentController : ControllerBase
    {

        private readonly IConfigurationDbContext _configurationContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppSettings _settings;

        public ServiceDeploymentController(IConfigurationDbContext configurationContext, RoleManager<IdentityRole> roleManager, IOptions<AppSettings> settings)
        {
            _configurationContext = configurationContext;
            _roleManager = roleManager;
            _settings = settings.Value;
        }

        
        [HttpPost("register")]
        public ServiceDeploymentResult RegisterService([FromBody] ServiceIdentityDetails serviceIdentityDetails)
        {
            var result = UpdateService(serviceIdentityDetails);
            return result;
        }

        private ServiceDeploymentResult UpdateService(ServiceIdentityDetails serviceIdentityDetails)
        {
            var summary = new Dictionary<string, string>();
            var result = new ServiceDeploymentResult();

            // use transaction to allow multiple SaveChanges() calls within one transaction
            // in case linked objects with auto-generated ids are updated
            try
            {
                using (var scope = new TransactionScope())
                {
                    var api = UpdateServiceBasicDetails(serviceIdentityDetails, summary);

                    // update api, or add it if it does not exists in db:
                    _configurationContext.ApiResources.Update(api);

                    // update api associated clients:
                    UpdateServiceAssociatedClients(api, serviceIdentityDetails.ClientDetails, summary);


                    //update service identity roles
                    UpdateServiceRoles(serviceIdentityDetails.ApiRoles);

                    _configurationContext.SaveChanges();
                    scope.Complete();

                    result.Success = true;
                }
            }
            catch (TransactionAbortedException ex)
            {
                Serilog.Log.Logger.Error(ex, "Failed to complete Service Deployment");
                result.Success = false;
                result.ErrorMessage = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
            }

            result.Summary = summary;
            return result;
        }

        private void UpdateServiceRoles(IEnumerable<string> apiRoles)
        {
            if (apiRoles == null)
                return;
            foreach (var role in apiRoles)
            {
                var role_exists = _roleManager.Roles.Any(r => !string.IsNullOrEmpty(r.Name)
                            && r.Name.Equals(role, StringComparison.InvariantCultureIgnoreCase));

                if (!role_exists)
                {
                    var result = _roleManager.CreateAsync(new IdentityRole(role)).Result;
                    if (!result.Succeeded)
                    {
                        string errors = string.Join(" \n", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                        throw new ServiceDeploymentException($"Failed to create user role [{role}].\n{errors}");
                    }
                }
            }
        }

        private void UpdateServiceAssociatedClients(ApiResource api, IEnumerable<ServiceClientDetails> clientsDetails, Dictionary<string, string> summary)
        {
            // get all clients associated with this API (example: hosted services clients)
            var api_clients = _configurationContext.Clients
                .Include(c => c.Properties)
                .Include(c => c.ClientSecrets)
                .Include(c => c.AllowedScopes)
                .Include(c => c.Claims)
                .Include(c => c.AllowedGrantTypes)
                .Where(c => c.Properties.Any(p => p.Key == ServiceDeploymentConstants.MASTER_API_NAME
                    && p.Value == api.Name)).ToList();

            var api_clients_to_delete = api_clients.Where(c =>
                !clientsDetails?.Any(cd => cd.ClientId == c.ClientId) ?? true);

            var api_clients_to_update = new List<Client>();           

            foreach (var client_info in clientsDetails ?? Enumerable.Empty<ServiceClientDetails>())
            {
                var client = api_clients.FirstOrDefault(c => c.ClientId
                    .Equals(client_info.ClientId, StringComparison.InvariantCultureIgnoreCase));

                if (client == null)
                {
                    client = new Client()
                    {
                        ClientId = client_info.ClientId,
                        ProtocolType = IdentityServerConstants.ProtocolTypes.OpenIdConnect,
                        RequireClientSecret = true,
                        RequireConsent = false,
                        RequirePkce = false,
                        AccessTokenLifetime = _settings.IdentityServer.AccessTokenLifetimeInSeconds,
                        EnableLocalLogin = true,
                        ClientClaimsPrefix = "client_",
                        Created = DateTime.Now
                    };

                    client.Properties = new List<ClientProperty>()
                    {
                        new ClientProperty()
                        {
                            Client = client,
                            Key = ServiceDeploymentConstants.MASTER_API_NAME,
                            Value = api.Name
                        }
                    };
                }

                client.AllowedGrantTypes = new List<ClientGrantType>()
                {
                    new ClientGrantType()
                    {
                        Client = client,
                        GrantType = OidcConstants.GrantTypes.ClientCredentials
                    }
                };

                client.ClientName = client_info.ClientName;

                // update client secret
                client.ClientSecrets = new List<ClientSecret> {
                    new ClientSecret() {
                        Client = client,
                        Value = client_info.ClientSecret.ToSha512(),
                        Created = DateTime.Now,
                        Expiration = null,
                        Type = IdentityServerConstants.SecretTypes.SharedSecret
                    }
                };

                //update client claims
                client.Claims = client_info.ClientClaims?.Select(cc => new ClientClaim()
                {
                    Client = client,
                    Type = cc.Key,
                    Value = cc.Value
                }).ToList();

                //update client scopes
                client.AllowedScopes = client_info.ClientAllowedScopes?.Split(new char[0], StringSplitOptions.RemoveEmptyEntries)
                    .Distinct().Select(cs => new ClientScope()
                    {
                        Client = client,
                        Scope = cs
                    }).ToList();

                client.Updated = DateTime.Now;
                api_clients_to_update.Add(client);
            }

            // remove deleted api clients if any:
            _configurationContext.Clients.RemoveRange(api_clients_to_delete);
            // update or add new api clients:
            _configurationContext.Clients.UpdateRange(api_clients_to_update);

            _configurationContext.SaveChanges();

            summary.Add("UpdatedApiClients", string.Join(", ", api_clients_to_update.Select(c => c.ClientId)));
            summary.Add("DeletedApiClients", string.Join(", ", api_clients_to_delete.Select(c => c.ClientId)));
        }

        private ApiResource UpdateServiceBasicDetails(ServiceIdentityDetails serviceIdentityDetails, Dictionary<string, string> summary)
        {
            var api = _configurationContext.ApiResources
                .Include(a => a.Scopes)
                .Include(a => a.UserClaims)
                .Include(a => a.Properties)
                .FirstOrDefault(a => a.Name == serviceIdentityDetails.ApiCode);

            if (api == null)
            {
                api = new ApiResource()
                {
                    Name = serviceIdentityDetails.ApiCode,
                    Enabled = true,
                    Created = DateTime.Now,
                    Properties = new List<ApiResourceProperty>(),
                    UserClaims = new List<ApiResourceClaim>(),
                    Scopes = new List<ApiScope>()
                };
            }

            api.DisplayName = serviceIdentityDetails.ApiDisplayName;
            api.Description = serviceIdentityDetails.ApiDescription;

            
            // add (or overwrite) scopes
            api.Scopes = serviceIdentityDetails.Scopes.Select(scopeName => new ApiScope()
            {
                ApiResource = api,
                Name = scopeName,
                ShowInDiscoveryDocument = !scopeName.Contains('.')
            }).ToList();

            

            // add (or overwrite) user claims
            var claims = new string[] { "sub", "name", "given_name", "family_name",
                "phone_number", "email", "inspection_center_id", "role" };

            api.UserClaims = claims.Select(claimType => new ApiResourceClaim()
            {
                ApiResource = api,
                Type = claimType
            }).ToList();
            serviceIdentityDetails.ApiUserClaims?.ToList().ForEach(c => {
                api.UserClaims.Add(new ApiResourceClaim() {
                    ApiResource = api,
                    Type = c
                });
            });


            api.Updated = DateTime.Now;

            summary.Add("Api", api.Name);
            summary.Add("Scopes", string.Join(", ", api.Scopes.Select(s => s.Name)));
            return api;
        }

    }
}

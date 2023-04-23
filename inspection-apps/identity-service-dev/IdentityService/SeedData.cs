// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityService.Migrations;
using IdentityService.Models;
using IdentityService.Models.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityService
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var appSettings = ((IOptions<AppSettings>)serviceProvider.GetService(typeof(IOptions<AppSettings>))).Value;

                DateTime dtNow = DateTime.Now;
                scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
                context.Database.Migrate();

                var usersdbcontext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                usersdbcontext.Database.Migrate();

                // Create Seed Roles
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();



                Console.WriteLine("Seeding database...");
                EnsureIdentitySeedData(context);
                EnsureIdentityServiceSelfSeedData(context, appSettings.ApiName);
                EnsureIdentityServiceClientsSeedData(context, appSettings.ClientsSettings);
                //EnsureClientSeedData(context);
                Console.WriteLine("Done seeding database.");
                Console.WriteLine();
            }
        }

        private static void EnsureIdentityServiceClientsSeedData(ConfigurationDbContext context, ServiceClientsSettings clientsSettings)
        {
            if (clientsSettings == null)
                return;
            foreach (var internalClient in clientsSettings.InternalClients)
            {
                var client = context.Clients.Include(c => c.ClientSecrets)
                    .Include(c => c.Properties)
                    .Include(c => c.Claims)
                    .Include(c => c.AllowedScopes)
                    .FirstOrDefault(c => c.ClientId == internalClient.ClientId);

                if (client == null)
                {
                    client = new Client
                    {
                        ClientId = internalClient.ClientId,
                        RequireConsent = false,
                        AccessTokenLifetime = 60 * 60 * 24,
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        AllowedScopes = context.ApiResources.SelectMany(a => a.Scopes.Select(s => s.Name))
                            .Where(s => s.EndsWith(internalClient.ClientScopeSuffix)).ToList(),
                        ClientClaimsPrefix = "client_",
                        Claims = new List<Claim>() { new Claim("sub", Guid.NewGuid().ToString()) }
                    }.ToEntity();
                }


                client.ClientSecrets = new[]
                {
                    new IdentityServer4.EntityFramework.Entities.ClientSecret
                    {
                        Client = client,
                        Value = internalClient.ClientSecret.ToSha512(),
                        Type = IdentityServer4.IdentityServerConstants.SecretTypes.SharedSecret
                    }
                }.ToList();

                client.AllowedScopes = context.ApiResources.SelectMany(a => a.Scopes.Select(s => s.Name))
                            .Where(s => s.EndsWith(internalClient.ClientScopeSuffix))
                            .Select(scope => new IdentityServer4.EntityFramework.Entities.ClientScope
                            {
                                Client = client,
                                Scope = scope,
                            }).ToList();

                client.Properties = new[] 
                {
                    new IdentityServer4.EntityFramework.Entities.ClientProperty
                    {
                        Client = client,
                        Key = ServiceDeployment.ServiceDeploymentConstants.SCOPE_SUFFIX,
                        Value = internalClient.ClientScopeSuffix
                    }
                }.ToList();

                context.Clients.Update(client);
                context.SaveChanges();
            }
        }

        private static void EnsureIdentityServiceSelfSeedData(ConfigurationDbContext context, string apiName)
        {
            if (string.IsNullOrWhiteSpace(apiName))
                throw new Exception("ApiName value is missing in appsettings.");

            if (!context.ApiResources.Any(api => api.Name == apiName))
            {
                context.ApiResources.Add(new ApiResource
                {
                    Name = apiName,
                    Scopes = {
                        new Scope(){ Name = apiName },
                        new Scope(){ Name = $"{apiName}.internal", ShowInDiscoveryDocument = false},
                        new Scope(){ Name = $"service.deployment", ShowInDiscoveryDocument = false},
                        new Scope(){ Name = $"service.internal", ShowInDiscoveryDocument = false},
                        new Scope(){ Name = $"service.admin", ShowInDiscoveryDocument = false}
                    }
                }.ToEntity());
                context.SaveChanges();
            }
        }

        private static void EnsureIdentitySeedData(IConfigurationDbContext context)
        {
            if (!context.IdentityResources.Any())
            {
                Console.WriteLine("IdentityResources being populated");
                foreach (var resource in Config.GetIdentityResources().ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }
        }

        private static void EnsureClientSeedData(IConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                Console.WriteLine("Clients being populated");
                foreach (var client in Config.GetClients().ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Clients already populated");
            }
        }
    }
}

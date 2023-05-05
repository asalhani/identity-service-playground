using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using App.IdentityServer.Configuration;
using App.IdentityServer.Migrations.IdentityServer;
using App.IdentityServer.Models;
using IdentityServer4.EntityFramework.Mappers;

namespace App.IdentityServer.Extensions
{
    public static class UsersAndRolesMigrationManager
    {
        public static IHost MigrateUsersAndRoles(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    try
                    {
                        context.Database.Migrate();

                        var employeeUser1 = new ApplicationUser()
                        {
                            Email = "asalhani@elm.sa",
                            UserName = "asalhani@elm.sa",
                            NormalizedEmail = "ASALHANI@ELM.SA",
                            NormalizedUserName = "ASALHANI@ELM.SA"
                        };
                        
                        var publicUser1 = new ApplicationUser()
                        {
                            Email = null,
                            UserName = "2088755802",
                            NormalizedEmail = null,
                            NormalizedUserName = "2088755802"
                        };
                        context.Users.Add(employeeUser1);
                        context.Users.Add(publicUser1);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }

            return host;
        }
    }
}
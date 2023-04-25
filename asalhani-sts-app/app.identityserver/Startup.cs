using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using App.IdentityServer.Configuration;
using Microsoft.AspNetCore.HttpOverrides;

namespace App.IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer(opt =>
                {
                    //  restrict the lifetime of a cookie so that after this time, the refresh token api will not work and the user has to login again
                    // Now, our application will refresh our token several times every sixty seconds, but after the cookie’s lifetime expires, the user will be forced to log in again.
                    opt.Authentication.CookieLifetime = TimeSpan.FromMinutes(4);
                    opt.IssuerUri = "https://localhost:5005";
                    opt.Events.RaiseErrorEvents = true;
                    opt.Events.RaiseInformationEvents = true;
                    opt.Events.RaiseFailureEvents = true;
                    opt.Events.RaiseSuccessEvents = true;

                    opt.UserInteraction.LoginReturnUrlParameter = "returnUrl";
                    opt.UserInteraction.LoginUrl = "http://localhost:4200/#/identity/login";
                    opt.UserInteraction.LogoutUrl = "http://localhost:4200/#/identity/logout";
                })
                .AddTestUsers(InMemoryConfig.GetUsers())
                .AddInMemoryClients(InMemoryConfig.GetClients())
                .AddInMemoryApiResources(InMemoryConfig.GetApiResources())
                .AddInMemoryApiScopes(InMemoryConfig.GetApiScopes())
                .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
                .AddDeveloperSigningCredential() //not something we want to use in a production environment;
                .AddProfileService<CustomProfileService>();
                // .AddConfigurationStore(opt =>
                //  {
                //      opt.ConfigureDbContext = c => c.UseSqlServer(Configuration.GetConnectionString("sqlConnection"),
                //          sql => sql.MigrationsAssembly(migrationAssembly));
                //  })
                // .AddOperationalStore(opt =>
                // {
                //     opt.ConfigureDbContext = o => o.UseSqlServer(Configuration.GetConnectionString("sqlConnection"),
                //         sql => sql.MigrationsAssembly(migrationAssembly));
                // });

            services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                     .AllowCredentials()
                    .AllowAnyHeader();
            }));
            
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseCors("corsapp");

            app.UseIdentityServer();
            app.UseAuthorization();
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

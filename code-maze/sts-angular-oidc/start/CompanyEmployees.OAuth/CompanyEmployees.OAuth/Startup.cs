using System;
using CompanyEmployees.OAuth.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace CompanyEmployees.OAuth
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
                })
                .AddTestUsers(InMemoryConfig.GetUsers())
                .AddDeveloperSigningCredential() //not something we want to use in a production environment;
                .AddProfileService<CustomProfileService>()
                .AddConfigurationStore(opt =>
                 {
                     opt.ConfigureDbContext = c => c.UseSqlServer(Configuration.GetConnectionString("sqlConnection"),
                         sql => sql.MigrationsAssembly(migrationAssembly));
                 })
                .AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = o => o.UseSqlServer(Configuration.GetConnectionString("sqlConnection"),
                        sql => sql.MigrationsAssembly(migrationAssembly));
                });

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

            app.UseIdentityServer();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

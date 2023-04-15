using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerApp;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // services.AddRazorPages();
        services.AddControllers();

        var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        const string connectionString = @"Server=localhost;Database=IdenittyServer;User Id=sa;Password=Aa123456;MultipleActiveResultSets=true;TrustServerCertificate=True";

        services.AddIdentityServer()
            .AddTestUsers(AppIdentityConfiguration.TestUsers)
            .AddDeveloperSigningCredential()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            });
        
        // services.AddIdentityServer()
        //     .AddInMemoryClients(AppIdentityConfiguration.Clients)
        //     .AddInMemoryIdentityResources(AppIdentityConfiguration.IdentityResources)
        //     .AddInMemoryApiResources(AppIdentityConfiguration.ApiResources)
        //     .AddInMemoryApiScopes(AppIdentityConfiguration.ApiScopes)
        //     .AddTestUsers(AppIdentityConfiguration.TestUsers)
        //     .AddDeveloperSigningCredential();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        app.UseItToSeedSqlServer();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });

        app.UseIdentityServer();
        
        app.Run();
    }
}
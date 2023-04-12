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

        // services.AddIdentityServer()
        //     .AddInMemoryApiResources(AppAppAppIdentityConfiguration.GetApis())
        //     .AddInMemoryClients(AppAppAppIdentityConfiguration.GetClients())
        //     .AddInMemoryApiScopes(AppAppAppIdentityConfiguration.GetScopes())
        //     .AddDeveloperSigningCredential();
        
        services.AddIdentityServer()
            .AddInMemoryClients(AppIdentityConfiguration.Clients)
            .AddInMemoryIdentityResources(AppIdentityConfiguration.IdentityResources)
            .AddInMemoryApiResources(AppIdentityConfiguration.ApiResources)
            .AddInMemoryApiScopes(AppIdentityConfiguration.ApiScopes)
            .AddTestUsers(AppIdentityConfiguration.TestUsers)
            .AddDeveloperSigningCredential();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
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
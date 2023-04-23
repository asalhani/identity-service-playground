// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.AccessTokenValidation;
using IdentityService.Business;
using IdentityService.Business.Abstract;
using IdentityService.DataAccess;
using IdentityService.DataAccess.Abstract;
using IdentityService.Helpers;
using IdentityService.Helpers.Abstract;
using IdentityService.Migrations;
using IdentityService.Models;
using IdentityService.Models.Configurations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using PlatformCommons.PlatformService.Abstractions.Notification;
using PlatformCommons.Service.Application.Configuration;
using PlatformCommons.Service.Domain.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using My.Extensions.Localization.Json;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;

namespace IdentityService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            Configuration = config;
            Environment = env;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IMemberManager, MemberManager>();
            services.AddScoped<IUserMessageManager, UserMessageManager>();
            services.AddScoped<IUserMessageRepository, UserMessageDA>();
            services.AddScoped<IAspNetPasswordHistoryRepository, AspNetPasswordHistoryDA>();
                              
            if (appSettings.Captcha.CaptchaType == "Google")
                services.AddScoped<ICaptchaValidator, RecaptchaValidator>();
            else
                services.AddScoped<ICaptchaValidator, CustomCaptchaValidator>();


            services.AddScoped<IDbConnection>(p => { return new SqlConnection(Configuration.GetConnectionString("DefaultConnection")); });

            services.AddCors();

            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(connectionString,
              sql => sql.MigrationsAssembly(migrationsAssembly).MigrationsHistoryTable("__EFMigrationsHistory", IdentityServiceConstants.DatabaseSchemaName)));



            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = appSettings.UserAccount.MaxFailedAccessAttemptsBeforeLockout;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(appSettings.UserAccount.LockoutTimeSpanInMinutes);
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<CookieAuthenticationOptions>(
                IdentityConstants.TwoFactorUserIdScheme, s =>
                {
                    s.ExpireTimeSpan = TimeSpan.FromMinutes(appSettings.UserAccount.OtpAttemptsTimeoutInMinutes);
                    //s.Cookie.Path = appSettings.IdentityServer.CookiePath;
                });
            services.Configure<CookieAuthenticationOptions>(
                IdentityConstants.ApplicationScheme, s =>
                {
                    //Prevent the browser from preserving the cookie after it get closed, so the
                    // user will be asked to login after closing the browser even he didn't sign out properly
                    s.Cookie.Expiration = TimeSpan.FromSeconds(1);
                    //This sets the ticket expiration time span. The default value is 14 days which is too much.
                    //Reducing this value minimizes the risk for someone to reuse the cookie if he managed to capture it
                    //This case is invalid if the user signed out properly as the security stamp will change and the cookie will become invalid.
                    //Note that this reduces the timespan window available for the oidc silent refresh process. This means that the value 
                    // indicated by client's AccessTokenLifetime should be less than the lifetime span of the cookie, else the cookie will expire
                    // and system will not be able to re-authenticate the user for generating a new OAuth token
                    s.ExpireTimeSpan = TimeSpan.FromMinutes(65);
                    //s.Cookie.Path = appSettings.IdentityServer.CookiePath;
                });
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                //This will force the server to validate the security stamp in the application cookie on nearly 
                // every request. This will preventint anyone from capturing the cookie and resuse it after the user signed out
                options.ValidationInterval = TimeSpan.Zero;
            });


            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
                options.AuthenticationDisplayName = "Windows";
            });

            

            var identityServer = services.AddIdentityServer(options =>
            {
                options.IssuerUri = appSettings.IdentityServer.IssuerUri;
                options.PublicOrigin = appSettings.IdentityServer.PublicOrigin;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.UserInteraction.LoginReturnUrlParameter = "returnUrl";
                options.UserInteraction.LoginUrl = $"{appSettings.IdentityServer.IssuerUri}{appSettings.IdentityServer.LoginPageUrl}";
                options.UserInteraction.LogoutUrl = $"{appSettings.IdentityServer.IssuerUri}{appSettings.IdentityServer.LogoutPageUrl}";
            })
                .AddAspNetIdentity<ApplicationUser>()
                // this adds the config data from DB (clients, resources, CORS)
                .AddConfigurationStore(options =>
                {
                    options.DefaultSchema = IdentityServiceConstants.DatabaseSchemaName;
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly).MigrationsHistoryTable("__EFMigrationsHistory", IdentityServiceConstants.DatabaseSchemaName));
                    
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.DefaultSchema = IdentityServiceConstants.DatabaseSchemaName;
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly).MigrationsHistoryTable("__EFMigrationsHistory", IdentityServiceConstants.DatabaseSchemaName));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    // options.TokenCleanupInterval = 15; // interval in seconds. 15 seconds useful for debugging
                })
                .AddProfileService<UserProfileService>()
                .AddExtensionGrantValidator<DelegationGrantValidator>();

            // Add Bearer token authentication for secured API calls
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = appSettings.IdentityServiceURL;
                    options.RequireHttpsMetadata = false;
                    //options.ApiName = appSettings.ApiName;
                    options.IntrospectionDiscoveryPolicy.ValidateIssuerName = false;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Deployment", p =>
                {
                    p.RequireScope("identityservice.deployment");
                    p.AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme);
                    p.RequireAuthenticatedUser();
                });

                options.AddPolicy("DefaultPolicy", p =>
                {
                    p.RequireScope(
                        appSettings.ApiName
                        , $"{appSettings.ApiName}.internal"
                        , "openid"
                        , "inspection_profile"
                        , "service.internal"
                    );
                    p.AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme);
                    p.RequireAuthenticatedUser();
                });

                options.DefaultPolicy = options.GetPolicy("DefaultPolicy");
            });

            IdentityModelEventSource.ShowPII = true;

            services.AddScoped<IAuthorizationTokenProvider, ISAuthorizationTokenProvider>();
            services.AddScoped<IRefitServiceResolver, RefitServiceResolver>();
            services.AddRefitService<IEmailNotification>(appSettings.Notifications.NotificationServiceUrl);
            services.AddRefitService<ISmsNotification>(appSettings.Notifications.NotificationServiceUrl);

            // configure data protection service 
            if (appSettings.DataProtection.IsRequired)
            {
                X509Certificate2 certificate = null;
                if (Environment.EnvironmentName == "docker")
                {
                    certificate = new X509Certificate2(appSettings.DataProtection.CertificateFilePath, appSettings.DataProtection.CertificatePassword);
                }
                else
                {
                    X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadOnly);
                    var certificates = store.Certificates.Find(X509FindType.FindByIssuerName, appSettings.DataProtection.CertificateIssuerName, true);
                    if (certificates != null && certificates.Count > 0)
                        certificate = certificates[0];
                }

                if (certificate != null)
                    services
                           .AddDataProtection()
                           .PersistKeysToFileSystem(new DirectoryInfo(appSettings.DataProtection.KeyFolderName))
                             .ProtectKeysWithCertificate(certificate);
                // .SetApplicationName("ISPAPP"); 
            }



            // Dependency HTTPContext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // cookie policy to deal with temporary browser incompatibilities
            services.AddSameSiteCookiePolicy();


            if (Environment.IsDevelopment() || Environment.EnvironmentName == "Dev")
            {
                identityServer.AddDeveloperSigningCredential();
            }
            else
            {
                if (Environment.EnvironmentName == "docker")
                    identityServer.AddCertificateFromFile(appSettings.IdentityServer, Serilog.Log.Logger);
                else
                    identityServer.AddCertificateFromStore(appSettings.IdentityServer.SigninKeyCredentials);
            }

            services.AddJsonLocalization(options =>
            {
                options.ResourcesPath = "wwwroot";
                options.ResourcesType = ResourcesType.CultureBased;
            });

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("ar");
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture("ar");
        }

        public void Configure(IApplicationBuilder app)
        {
            ////For debuging
            //app.Use(async (context, next) =>
            //{
            //    await next.Invoke();
            //});

            app.UseCookiePolicy();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("ar"),
                new CultureInfo("en")
            };
            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ar", "ar"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
            app.UseRequestLocalization(options);


            app.UseIdentityServer();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = p =>
                {
                    p.Context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    p.Context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                    p.Context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                }
            });
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });
            app.UseMvcWithDefaultRoute();
        }
    }
    public static class IdentityServerExtension
    {
        public static void AddCertificateFromStore(this IIdentityServerBuilder builder, string keyIssuer)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            var certificates = store.Certificates.Find(X509FindType.FindByIssuerName, keyIssuer, true);

            if (certificates.Count > 0)
                builder.AddSigningCredential(certificates[0]);


        }

        public static void AddCertificateFromFile(this IIdentityServerBuilder builder, IdentityServerSettings settings, Serilog.ILogger logger)
        {
            if (File.Exists(settings.CertificateFilePath))
            {
                logger.Debug($"SigninCredentialExtension adding key from file {settings.CertificateFilePath}");

                // You can simply add this line in the Startup.cs if you don't want an extension. 
                // This is neater though ;)
                builder.AddSigningCredential(new X509Certificate2(settings.CertificateFilePath, settings.CertificatePassword));
            }
            else
            {
                logger.Error($"SigninCredentialExtension cannot find key file {settings.CertificateFilePath}");
            }
        }

        public static IServiceCollection AddSameSiteCookiePolicy(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                //options.MinimumSameSitePolicy = (SameSiteMode)(-1);
                options.OnAppendCookie = cookieContext => 
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext => 
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });

            return services;
        }
        
        private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                if (!httpContext.Request.IsHttps || DisallowsSameSiteNone(userAgent))
                {
                    // For .NET Core < 3.1 set SameSite = (SameSiteMode)(-1)
                    options.SameSite = (SameSiteMode)(-1);
                }
            }
        }

        private static bool DisallowsSameSiteNone(string userAgent)
        {
            // Cover all iOS based browsers here. This includes:
            // - Safari on iOS 12 for iPhone, iPod Touch, iPad
            // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the iOS networking stack
            if (userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack. This includes:
            // - Safari on Mac OS X.
            // This does not include:
            // - Chrome on Mac OS X
            // Because they do not use the Mac OS networking stack.
            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") && 
                userAgent.Contains("Version/") && userAgent.Contains("Safari"))
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None, 
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions, 
            // but pre-Chromium Edge does not require SameSite=None.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }
    }
}

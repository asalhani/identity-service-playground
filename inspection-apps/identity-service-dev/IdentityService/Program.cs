// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using DbUp;
using IdentityService.Models.Configurations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlatformCommons.DeploymentTools;
using PlatformCommons.DeploymentTools.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace IdentityService
{
    public class Program
    {
        private static DeploymentToolExitCode PerformServiceDbSetup(IWebHost host, string[] args)
        {
            try
            {
                var config = host.Services.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("DefaultConnection");

                IsDatabaseServerReady(config, connectionString);

                SeedData.EnsureSeedData(host.Services);
                var databaseFolder = args[1];
                var fullDatabasePath = PlatformCommons.Service.Domain.Utils.FileUtils.GetFullPath(databaseFolder, Environment.CurrentDirectory);

                SeedData.EnsureSeedData(host.Services);
                
                var appSettings = ((IOptions<AppSettings>)host.Services.GetService(typeof(IOptions<AppSettings>))).Value;
                var upgrader = DeployChanges.To
                    .SqlDatabase(connectionString, IdentityServiceConstants.DatabaseSchemaName)
                    .WithScriptsFromFileSystem(fullDatabasePath)
                    .WithVariables(new Dictionary<string, string>() {
                    { "ClientName" ,appSettings.ClientDatabaseConfig.ClientName },
                    { "ClientNameId" ,appSettings.ClientDatabaseConfig.ClientNameId },
                    { "ClientUrl" ,appSettings.ClientDatabaseConfig.ClientUrl },
                    { "AdminEmail" , appSettings.ClientDatabaseConfig.AdminEmail}
                    })
                    .LogToConsole()
                    .Build();
                var result = upgrader.PerformUpgrade();

                if (!result.Successful)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(result.Error);
                    Console.ResetColor();
                    Log.CloseAndFlush();
                    return DeploymentToolExitCode.DbUpUpgradeFaild;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
                Console.ResetColor();
                return DeploymentToolExitCode.Success;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
                return DeploymentToolExitCode.UnknownError;
            }

        }

        private static void IsDatabaseServerReady(IConfiguration config, string connectionString)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                var installManager = new ServiceInstallManager(config);
                installManager.IsDependancyUp(builder.DataSource, 1433);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }
        }

        public static void Main(string[] args)
        {
            // -ci-skip-run-host /../Database
            Console.Title = "InspectionPlatform.IdentityServer";

            var isContiniousIntegrationMode = args != null && args.Contains("-ci");
            var isContiniousIntegrationModeWithSkipRunHost = args != null && args.Contains("-ci-skip-run-host");

            SetupLogger();
            var host = BuildWebHost(args);

            if (isContiniousIntegrationMode || isContiniousIntegrationModeWithSkipRunHost)
            {
               var deploymentResult = PerformServiceDbSetup(host, args);
                if (isContiniousIntegrationMode)
                {
                    Console.WriteLine("Starting .NET Core Host run");
                    host.Run();
                }
                else // isContiniousIntegrationModeWithSkipRunHost
                {
                    Console.WriteLine("Skipping .NET Core Host run");
                    Environment.Exit((int) deploymentResult);
                }
            }
            else
            {
                Console.WriteLine("Starting .NET Core Host run");
                host.Run();
            }
        }
        private static IConfiguration GetConfig()
        {
            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional:false)
                .AddJsonFile($"appsettings.{currentEnv}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseSerilog()
                    .UseConfiguration(GetConfig())
                    .Build();
        }

        private static void SetupLogger()
        {
            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                .AddJsonFile($"appsettings.{currentEnv}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
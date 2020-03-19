using System;
using ACVPCore;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Common;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Common.Services;
using NIST.CVP.Common.Targets;
using NLog.Targets;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Register the NLog -> Serilog target so Serilog can intercept NLog log calls.
            Target.Register<SerilogTarget>(SerilogTarget.TargetName); 
            SerilogTarget.ReplaceAllNLogTargetsWithSingleSerilogForwarder();
            
            var directoryConfig = EntryPointConfigHelper.GetRootDirectory();
            CreateHostBuilder(args, directoryConfig).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args, string directoryConfig) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    if (string.IsNullOrWhiteSpace(env))
                    {
                        /* TODO this could fall back to an environment,
                         * when/if driver is updated to check for var
                         */
                        throw new Exception("ASPNETCORE_ENVIRONMENT env variable not set.");
                    }

                    context.HostingEnvironment.EnvironmentName = env;
                    
                    builder
                        .AddJsonFile($"{directoryConfig}sharedappsettings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"{directoryConfig}sharedappsettings.{env}.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"{directoryConfig}appsettings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"{directoryConfig}appsettings.{env}.json", optional: false, reloadOnChange: false);
                })
                .UseWindowsService()
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // CVP.DB
                    services.InjectDatabaseInterface();
                    
                    // ACVPCore
                    services.InjectACVPCore();
                    
                    // Orleans
                    services.AddTransient<IDbConnectionStringFactory, DbConnectionStringFactory>();
                    services.AddTransient<IDbConnectionFactory, SqlDbConnectionFactory>();

                    // Pools
                    services.AddHttpClient();
                    
                    // Setup Configs
                    services.Configure<TaskQueueProcessorConfig>(hostContext.Configuration.GetSection(nameof(TaskQueueProcessorConfig)));
                    services.Configure<EnvironmentConfig>(hostContext.Configuration.GetSection(nameof(EnvironmentConfig)));
                    services.Configure<PoolConfig>(hostContext.Configuration.GetSection(nameof(PoolConfig)));
                    services.Configure<OrleansConfig>(hostContext.Configuration.GetSection(nameof(OrleansConfig)));

                    // TQP
                    services.InjectTaskQueueProcessorInterfaces();
                });
    }
}
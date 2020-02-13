using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Services;
using NIST.CVP.Common.Targets;
using NIST.CVP.Crypto.Oracle;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Interfaces;
using NIST.CVP.Pools.Services;
using Serilog;

namespace NIST.CVP.PoolAPI
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // Register the NLog -> Serilog target so Serilog can intercept NLog log calls.
            Target.Register<SerilogTarget>(SerilogTarget.TargetName); 
            SerilogTarget.ReplaceAllNLogTargetsWithSingleSerilogForwarder();
            
            var directoryConfig = EntryPointConfigHelper.GetRootDirectory();
            await CreateHostBuilder(args, directoryConfig).Build().RunAsync();
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
                .UseSerilog((context, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    
                    services.Configure<EnvironmentConfig>(hostContext.Configuration.GetSection(nameof(EnvironmentConfig)));
                    services.Configure<PoolConfig>(hostContext.Configuration.GetSection(nameof(PoolConfig)));
                    services.Configure<OrleansConfig>(hostContext.Configuration.GetSection(nameof(OrleansConfig)));

                    services.AddSingleton<IDbConnectionStringFactory, DbConnectionStringFactory>();
                    services.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();

                    services.AddSingleton<IJsonConverterProvider, JsonConverterProvider>();
                    services.AddSingleton<IPoolFactory, PoolFactory>();
                    services.AddSingleton<IPoolObjectFactory, PoolObjectFactory>();
                    services.AddSingleton<IPoolRepositoryFactory, PoolSqlRepositoryFactory>();
                    services.AddSingleton<IPoolLogRepository, PoolLogSqlRepository>();
                    services.AddSingleton<IOracle, OracleMinimalLoadSheddingRetries>();

                    services.AddSingleton<PoolManager>();
                })
                .ConfigureWebHostDefaults(builder => { builder.UseStartup<Startup>(); });        
    }
}

using Microsoft.AspNetCore.Hosting;
using NLog.Targets;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
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
            
            var (env, configurationRoot, poolConfig) = PreBuildBuilder(directoryConfig);

            await CreateHostBuilder(args, env, configurationRoot, poolConfig).Build().RunAsync();
        }

        /// <summary>
        /// Some of the bootstrapping of the app actually relies on config values.
        /// I'm not sure if there's a good way to hook into the config at the point of app bootstrapping, 
        /// doesn't seem like it.  So to get what is needed, create a builder to load the config files
        /// and pass the subsequent IConfiguration to the actual meaty app bootstrapping.
        /// </summary>
        /// <param name="directoryConfig">The directory configuration files are located.</param>
        /// <returns>string representing the running environment, the configuration root, and pool configuration.</returns>
        private static (string env, IConfigurationRoot configurationRoot, PoolConfig poolConfig) PreBuildBuilder(
            string directoryConfig)
        {
            var env = GetEnvironmentName();
            var tempConfigBuilder = new ConfigurationBuilder();
            AddJsonFiles(directoryConfig, tempConfigBuilder, env);
            var configurationRoot = tempConfigBuilder.Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.Configure<PoolConfig>(configurationRoot.GetSection(nameof(PoolConfig)));
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var poolConfig = serviceProvider.GetService<IOptions<PoolConfig>>().Value;
            return (env, configurationRoot, poolConfig);
        }
        
        private static string GetEnvironmentName()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(env))
            {
                /* TODO this could fall back to an environment,
                         * when/if driver is updated to check for var
                         */
                throw new Exception("ASPNETCORE_ENVIRONMENT env variable not set.");
            }

            return env;
        }
        
        private static void AddJsonFiles(string directoryConfig, IConfigurationBuilder builder, string env)
        {
            builder
                .AddJsonFile($"{directoryConfig}sharedappsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"{directoryConfig}sharedappsettings.{env}.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"{directoryConfig}appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"{directoryConfig}appsettings.{env}.json", optional: false, reloadOnChange: false);
        }

        private static IHostBuilder CreateHostBuilder(string[] args, string env, IConfigurationRoot configurationRoot, PoolConfig poolConfig) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    context.HostingEnvironment.EnvironmentName = env;
                    builder.AddConfiguration(configurationRoot);
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
                    services.AddSingleton(new JsonConverterProvider().GetJsonConverters());
                    services.AddSingleton<IPoolFactory, PoolFactory>();
                    services.AddSingleton<IPoolObjectFactory, PoolObjectFactory>();
                    services.AddSingleton<IPoolRepositoryFactory, PoolSqlRepositoryFactory>();
                    services.AddSingleton<IPoolLogRepository, PoolLogSqlRepository>();
                    services.AddSingleton<IOracle, OracleMinimalLoadSheddingRetries>();

                    services.AddSingleton<IPoolRepository<IResult>, PoolSqlRepository<IResult>>();
                    
                    services.AddSingleton<PoolManager>();
                })
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseUrls($"http://*:{poolConfig.Port}");
                    builder.UseStartup<Startup>();
                });
    }
}

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NIST.CVP.Pools;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Oracle.Builders;
using NIST.CVP.Crypto.Oracle.Exceptions;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.PoolAPI
{
    public static class Program
    {
        public const string OrleansPoolLogLocation = @"C:\Temp\poolOrleansLogs.json";

        public static PoolManager PoolManager { get; set; }
        public static List<PoolOrleansJob> PoolOrleansJobLog { get; private set; }

        private static IServiceProvider ServiceProvider { get; }

        static Program()
        {
            ServiceProvider = EntryPointConfigHelper.Bootstrap(AppDomain.CurrentDomain.BaseDirectory);
        }

        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());

            SetupPoolOrleansLog();

            var logConfig = new LoggingConfiguration();
            var logFile = new FileTarget("poolLogs") { FileName = @"C:\Temp\poolLogs.log" };
            var logConsole = new ConsoleTarget("logConsole");

            logConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, logConsole);
            logConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);

            LogManager.Configuration = logConfig;
            var logger = LogManager.GetCurrentClassLogger();

            var poolConfigFile = "poolConfig.json";
            string poolDirectory = GetPoolDirectory(args, isService);

            logger.Info($"PoolConfig: {poolConfigFile}");
            logger.Info($"PoolDirectory: {poolDirectory}");
            logger.Info("Loading pools from config file...");

            try
            {
                PoolManager = new PoolManager(
                    ServiceProvider.GetService<IOptions<PoolConfig>>(),
                    new OracleBuilder().Build(), // TODO this is super not the way to do this, figure out the proper way to do DI with this "form factor" of webapi/service.
                    poolConfigFile,
                    poolDirectory
                );
                logger.Info("Successfully connected to Orleans");
                PoolManager.LoadPools().FireAndForget();
            }
            catch (OrleansInitializationException)
            {
                logger.Fatal("Failed connecting to Orleans, is the service available?");
                return;
            }

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                builder.UseContentRoot(pathToContentRoot);
                builder.UseStartup<Startup>();
                builder.UseUrls("http://+:5002");
            }

            var host = builder.Build();

            if (isService)
            {
                logger.Info("Running as service");
                host.RunAsCustomService();
            }
            else
            {
                logger.Info("Running from console");
                host.Run();
            }
        }

        private static void SetupPoolOrleansLog()
        {
            if (File.Exists(OrleansPoolLogLocation))
            {
                PoolOrleansJobLog = JsonConvert.DeserializeObject<List<PoolOrleansJob>>(
                    File.ReadAllText(OrleansPoolLogLocation), 
                    new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        TypeNameHandling = TypeNameHandling.All
                    }
                );
            }
            else
            {
                PoolOrleansJobLog = new List<PoolOrleansJob>();
            }
        }

        /// <summary>
        /// Pool directory is "./Pools/" when not running as service,
        /// when running as service, the pool directory should be passed into the
        /// service registration.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="isService"></param>
        /// <returns></returns>
        private static string GetPoolDirectory(string[] args, bool isService)
        {
            if (isService)
            {
                return args.Last();
            }
            
            return Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\Pools");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}

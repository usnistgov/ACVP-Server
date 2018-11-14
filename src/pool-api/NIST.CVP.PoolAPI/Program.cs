using System;
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
using NIST.CVP.Crypto.Oracle.Builders;

namespace NIST.CVP.PoolAPI
{
    public static class Program
    {
        public static PoolManager PoolManager { get; set; }
        private static IServiceProvider ServiceProvider { get; }

        static Program()
        {
            ServiceProvider = EntryPointConfigHelper.Bootstrap(AppDomain.CurrentDomain.BaseDirectory);
        }

        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());

            var logConfig = new LoggingConfiguration();
            var logFile = new FileTarget("poolLogs") { FileName = @"C:\Temp\poolLogs.log" };
            var logConsole = new ConsoleTarget("logConsole");

            logConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, logConsole);
            logConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);

            LogManager.Configuration = logConfig;
            var logger = LogManager.GetCurrentClassLogger();

            var poolConfigFile = "poolConfig.json";
            var poolDirectory = args.Last();

            logger.Info($"PoolConfig: {poolConfigFile}");
            logger.Info($"PoolDirectory: {poolDirectory}");
            logger.Info("Loading pools from config file...");
            PoolManager = new PoolManager(
                ServiceProvider.GetService<IOptions<PoolConfig>>(), 
                new OracleBuilder().Build(), // TODO this is super not the way to do this, figure out the proper way to do DI with this "form factor" of webapi/service.
                poolConfigFile, 
                poolDirectory
            );
            logger.Info("Pools loaded.");
            
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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}

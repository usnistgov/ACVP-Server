using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NIST.CVP.Pools;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NIST.CVP.PoolAPI
{
    public static class Program
    {
        public static PoolManager PoolManager { get; set; }

        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("-c"));
            var builder = CreateWebHostBuilder(args.Where(arg => arg != "-c").ToArray());

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

            PoolManager = new PoolManager(poolConfigFile, poolDirectory);
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

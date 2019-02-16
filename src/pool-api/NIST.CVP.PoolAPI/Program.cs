using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NIST.CVP.Common.Helpers;

namespace NIST.CVP.PoolAPI
{
    public static class Program
    {
        private static bool _isService;
        
        public static void Main(string[] args)
        {
            _isService = !(Debugger.IsAttached || args.Contains("--console"));
            var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());

            var logConfig = new LoggingConfiguration();
            var logFile = new FileTarget("poolLogs") { FileName = @"C:\Temp\poolLogs.log" };
            var logConsole = new ConsoleTarget("logConsole");

            logConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, logConsole);
            logConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);

            LogManager.Configuration = logConfig;
            var logger = LogManager.GetCurrentClassLogger();

            if (_isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                builder.UseContentRoot(pathToContentRoot);
                builder.UseStartup<Startup>();
                builder.UseUrls("http://+:5002");
            }

            var host = builder.Build();

            if (_isService)
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
            Console.WriteLine(Directory.GetCurrentDirectory());

            return WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(GetRootDirectory())
                .UseStartup<Startup>();
        }

        private static string GetRootDirectory()
        {
            if (_isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                return Path.GetDirectoryName(pathToExe) + @"\";
            }

            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}

using System;
using System.IO;
using Autofac;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.AES_CBC;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace AES_CBC
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("No arguments supplied");
                return 1;
            }
            var requestFile = args[0];
            ConfigureLogging(requestFile);
            Logger.Info($"Generating Test Vectors for {requestFile}");

            //get generator and call it
            Logger.Debug("Generating");

            try
            {
                AutofacConfig.IoCConfiguration();
                using (var scope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var gen = scope.Resolve<Generator<Parameters, TestVectorSet>>();
                    var result = gen.Generate(requestFile);
                    if (!result.Success)
                    {
                        Console.Error.WriteLine(
                            $"ERROR! Generating Test Vectors for {requestFile}: {result.ErrorMessage}");
                        Logger.Error($"ERROR! Generating Test Vectors for {requestFile}: {result.ErrorMessage}");
                        //Console.ReadLine();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ERROR! Generating Test Vectors for {requestFile}: {ex.Message}");
                return 1;
            }


            Logger.Debug($"Success! Generating Test Vectors for {requestFile}");
            //Console.ReadLine();

            return 0;
        }

        private static Logger Logger
        {
            get { return LogManager.GetLogger("Generate"); }
        }

        private static void ConfigureLogging(string requestFile)
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            string baseDir = Path.GetDirectoryName(requestFile);
            fileTarget.FileName = Path.Combine(baseDir, "aes-cbc.log");
            fileTarget.Layout = "${longdate} ${level} ${logger} ${message}";
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, fileTarget));

            var consoleTarget = new ConsoleTarget("Console");
            consoleTarget.Layout = "${longdate} ${level} ${logger} ${message}";
            config.AddTarget(consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));
            LogManager.Configuration = config;
        }
    }
}

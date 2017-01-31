using System;
using System.IO;
using Autofac;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.AES_OFB;
using NIST.CVP.Generation.Core;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace AES_OFB_Val
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.Error.WriteLine("Not enough arguments supplied, must supply paths for result, prompt and and answer files");
                return 1;
            }
            var resultFile = args[0];
            var promptFile = args[1];
            var answerFile = args[2];
            LoggingHelper.ConfigureLogging(resultFile, "aes-ofb-val");
            Logger.Info($"Validating test results for {resultFile}");
            try
            {
                AutofacConfig.IoCConfiguration();
                using (var scope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var validator = scope.Resolve<Validator<TestVectorSet, TestCase>>();
                    var result = validator.Validate(resultFile, answerFile, promptFile);
                    if (!result.Success)
                    {
                        Console.Error.WriteLine($"ERROR! Validating Test Vectors for {resultFile}: {result.ErrorMessage}");
                        Logger.Error($"ERROR! Validating Test Vectors for {resultFile}: {result.ErrorMessage}");
                        //Console.ReadLine();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ERROR! Validating Test Vectors for {resultFile}: {ex.Message}");
                return 1;
            }

            Logger.Info($"Success! Validating Test Results for {resultFile}");
            //Console.ReadLine();

            return 0;
        }

        private static Logger Logger
        {
            get { return LogManager.GetLogger("Validate"); }
        }

        private static void ConfigureLogging(string requestFile)
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            string baseDir = Path.GetDirectoryName(requestFile);
            fileTarget.FileName = Path.Combine(baseDir, "aes-ofb-val.log");
            fileTarget.Layout = "${longdate} ${level} ${logger} ${message}";
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            var consoleTarget = new ConsoleTarget("Console");
            consoleTarget.Layout = "${longdate} ${level} ${logger} ${message}";
            config.AddTarget(consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));
            LogManager.Configuration = config;
        }
    }
}

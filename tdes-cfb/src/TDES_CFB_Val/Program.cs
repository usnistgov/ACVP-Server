using System;
using Autofac;
using Microsoft.Extensions.PlatformAbstractions;
//using NIST.CVP.Crypto.Core;
using NLog;
using NIST.CVP.Generation.Core;
using NIST.CVP.Crypto.TDES_CFB;
using Algo = NIST.CVP.Crypto.TDES_CFB.Algo;
using EnumEx = NIST.CVP.Crypto.TDES_CFB.EnumEx;

namespace TDES_CFB_Val
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
            var algoStr = args[0];
            var resultFile = args[1];
            var promptFile = args[2];
            var answerFile = args[3];

            LoggingHelper.ConfigureLogging(resultFile, "tdes-ofb-val");
            Logger.Info($"Running {PlatformServices.Default.Application.ApplicationName} { PlatformServices.Default.Application.ApplicationVersion}");

            Logger.Info($"Validating test results for {resultFile}");
            try
            {
                var algo = EnumEx.FromDescription<Algo>(algoStr);
                AutofacConfig.IoCConfiguration(algo);
                using (var scope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var validator = scope.Resolve<IValidator>();
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
    }
}

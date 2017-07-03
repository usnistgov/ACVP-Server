using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.RSA_KeyGen;
using NLog;

namespace RSA_KeyGen_Val
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
            
            LoggingHelper.ConfigureLogging(resultFile, "rsa-keygen-val");
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
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ERROR! Validating Test Vectors for {resultFile}: {ex.Message}\n{ex.StackTrace}");
                return 1;
            }

            Logger.Info($"Success! Validating Test Results for {resultFile}");
            return 0;
        }

        private static Logger Logger
        {
            get { return LogManager.GetLogger("Validate"); }
        }
    }
}

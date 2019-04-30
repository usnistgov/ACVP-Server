using System;
using CommandLineParser.Exceptions;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core.Exceptions;
using NIST.CVP.ParameterChecker.Helpers;
using NLog;

namespace NIST.CVP.ParameterChecker
{
    public class Program
    {
        public static Logger Logger => LogManager.GetLogger("ParameterChecker");

        public static int Main(string[] args)
        {
            var argumentParser = new ArgumentParsingHelper();

            try
            {
                var parsedParameters = argumentParser.Parse(args);

                // Get the IOC container for the algo
                AutofacConfig.IoCConfiguration(parsedParameters.Algorithm, parsedParameters.Mode, parsedParameters.Revision);

                using (var scope = AutofacConfig.GetContainer().BeginLifetimeScope())
                {
                    var checkRunner = new ParameterCheckRunner(scope);
                    checkRunner.ConfigureLogging(parsedParameters);
                    return checkRunner.Run(parsedParameters);
                }
            }
            catch (CommandLineException ex)
            {
                var errorMessage = $"ERROR: {ex.Message}";
                Console.WriteLine(errorMessage);
                Console.WriteLine(ex.StackTrace);
                Logger.Error($"Status Code: {StatusCode.CommandLineError}");
                Logger.Error(errorMessage);
                argumentParser.ShowUsage();
                return (int)StatusCode.CommandLineError;
            }
            catch (AlgoModeRevisionException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Logger.Fatal(ex);
                return (int)StatusCode.ModeError;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Logger.Fatal(ex);
                return (int)StatusCode.Exception;
            }
        }
    }
}

using CommandLineParser.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Common.Enums;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Exceptions;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Generation.GenValApp.Models;
using NLog;
using System;
using NIST.CVP.Generation.Core.Helpers;
using RunningOptionsHelper = NIST.CVP.Generation.GenValApp.Helpers.RunningOptionsHelper;

namespace NIST.CVP.Generation.GenValApp
{
    public static class Program
    {
        public static IServiceProvider ServiceProvider { get; }
        public static readonly string RootDirectory = AppDomain.CurrentDomain.BaseDirectory;

        static Program()
        {
            var configurationRoot = EntryPointConfigHelper.GetConfigurationRoot(RootDirectory);
            var serviceCollection = EntryPointConfigHelper.GetBaseServiceCollection(configurationRoot);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// The logger to utilize throughout the application run
        /// </summary>
        public static Logger Logger => LogManager.GetLogger("GenValApp");

        /// <summary>
        /// Entry point into application
        /// </summary>
        /// <param name="args">
        ///     Arguments used within app run, 
        ///     see <see cref="ArgumentParsingTarget"/> for details.
        /// </param>
        /// <returns></returns>
        public static int Main(string[] args)
        {
            var argumentParser = new ArgumentParsingHelper();

            try
            {
                var parsedParameters = argumentParser.Parse(args);
                var runningOptions = RunningOptionsHelper.GetRunningOptions(parsedParameters);
                ConfigureLogging(parsedParameters, runningOptions);

                Logger.Info($"Running in {runningOptions.GenValMode} mode for {EnumHelpers.GetEnumDescriptionFromEnum(runningOptions.AlgoMode)}");

                // Get the IOC container for the algo
                AutofacConfig.IoCConfiguration(ServiceProvider, runningOptions.AlgoMode);

                using (var scope = AutofacConfig.GetContainer().BeginLifetimeScope())
                {
                    var genValRunner = new GenValRunner(scope);
                    return genValRunner.Run(parsedParameters, runningOptions.GenValMode);
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

        /// <summary>
        /// Configure logging for the app run.
        /// </summary>
        /// <param name="parsedParameters">The parsed arguments into the app</param>
        /// <param name="runningOptions">The algorithm and running mode.</param>
        private static void ConfigureLogging(ArgumentParsingTarget parsedParameters, GenValRunningOptions runningOptions)
        {
            string filePath;

            switch (runningOptions.GenValMode)
            {
                case GenValMode.Generate:
                    filePath = parsedParameters.RegistrationFile.FullName;
                    break;
                case GenValMode.Validate:
                    filePath = parsedParameters.AnswerFile.FullName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid {nameof(GenValMode)}");
            }

            var logName = $"{runningOptions.GenValMode}";

            LoggingHelper.ConfigureLogging(filePath, logName);
        }
    }
}

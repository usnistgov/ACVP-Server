using CommandLineParser.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Common.Enums;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Generation.GenValApp.Models;
using NLog;
using System;
using System.IO;
using NIST.CVP.Generation.Core.Exceptions;

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

                // Get the IOC container for the algo
                AutofacConfig.IoCConfiguration(ServiceProvider, parsedParameters.Algorithm, parsedParameters.Mode, parsedParameters.Revision);

                using (var scope = AutofacConfig.GetContainer().BeginLifetimeScope())
                {
                    var genValRunner = new GenValRunner(scope);
                    genValRunner.SetRunningMode(parsedParameters);
                    genValRunner.ConfigureLogging(parsedParameters);
                    return genValRunner.Run(parsedParameters);
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
                return (int) StatusCode.CommandLineError;
            }
            catch (AlgoModeRevisionException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Logger.Fatal(ex);
                return (int) StatusCode.ModeError;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Logger.Fatal(ex);
                return (int) StatusCode.Exception;
            }
        }
    }
}

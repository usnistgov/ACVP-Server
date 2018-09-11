using CommandLineParser.Exceptions;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Generation.GenValApp.Models;
using NLog;

namespace NIST.CVP.Generation.GenValApp
{
    public static class Program
    {
        private const string SETTINGS_FILE = "appSettings";
        private const string SETTINGS_EXTENSION = "json";
        private static string FileDirectory;

        public static IServiceProvider ServiceProvider { get; }
        public static readonly string RootDirectory = AppDomain.CurrentDomain.BaseDirectory;

        static Program()
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(env))
            {
                /* TODO this could fall back to an environment,
                 * when/if driver is updated to check for var
                 */
                throw new Exception("ASPNETCORE_ENVIRONMENT env variable not set.");
            }

            Console.WriteLine($"Bootstrapping application using environment {env}");
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{RootDirectory}{SETTINGS_FILE}.{SETTINGS_EXTENSION}", optional: false, reloadOnChange: false)
                .AddJsonFile($"{RootDirectory}{SETTINGS_FILE}.{env}.{SETTINGS_EXTENSION}", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();
            serviceCollection.Configure<AlgorithmConfig>(configuration.GetSection(nameof(AlgorithmConfig)));

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
                FileDirectory = Path.GetPathRoot(parsedParameters.RegistrationFile.FullName);

                var dllLocation = RootDirectory;
                if (parsedParameters.DllLocation != null)
                {
                    dllLocation = parsedParameters.DllLocation.FullName;
                }

                // Get the IOC container for the algo
                AutofacConfig.IoCConfiguration(ServiceProvider, parsedParameters.Algorithm, parsedParameters.Mode, dllLocation);
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
                ErrorLogger.LogError(StatusCode.CommandLineError, "driver", ex.Message, FileDirectory);
                Console.WriteLine(errorMessage);
                Console.WriteLine(ex.StackTrace);
                Logger.Error($"Status Code: {StatusCode.CommandLineError}");
                Logger.Error(errorMessage);
                argumentParser.ShowUsage();
                return (int) StatusCode.CommandLineError;
            }
            catch (Exception ex)
            {
                var errorMessage = $"ERROR: {ex.Message}";
                ErrorLogger.LogError(StatusCode.Exception, "driver", ex.Message, FileDirectory);
                Console.WriteLine(errorMessage);
                Console.WriteLine(ex.StackTrace);
                Logger.Error($"Status Code: {StatusCode.Exception}");
                Logger.Error(errorMessage);
                return (int) StatusCode.Exception;
            }
        }
    }
}

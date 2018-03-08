using CommandLineParser.Exceptions;
using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Generation.GenValApp.Models;
using NLog;

namespace NIST.CVP.Generation.GenValApp
{
    public static class Program
    {
        private const string _SETTINGS_FILE = "appSettings.json";
        private static readonly string RootDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly AlgorithmConfig Config;
        private static GenValMode _genValMode;

        /// <summary>
        /// Static constructor - bootstraps and sets configuration
        /// </summary>
        static Program()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"{RootDirectory}{_SETTINGS_FILE}", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();

                IConfigurationRoot configuration = builder.Build();

                Config = new AlgorithmConfig();
                configuration.Bind(Config);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// The logger to utilize throughout the application run
        /// </summary>
        private static Logger Logger => LogManager.GetLogger("GenValApp");

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
            var parser = new CommandLineParser.CommandLineParser();
            var parsedParameters = new ArgumentParsingTarget();

            try
            {
                parser.ExtractArgumentAttributes(parsedParameters);

                args = GetArgsWhenNotProvided(args, parser);

                parser.ParseCommandLine(args);

                var dllLocation = RootDirectory;
                if (parsedParameters.DllLocation != null)
                {
                    dllLocation = parsedParameters.DllLocation.FullName;
                }

                SetRunningMode(parsedParameters);

                ConfigureLogging(parsedParameters);

                // Get the IOC container for the algo
                AutofacConfig.IoCConfiguration(Config, parsedParameters.Algorithm, parsedParameters.Mode, dllLocation);
                using (var scope = AutofacConfig.GetContainer().BeginLifetimeScope())
                {
                    return RunGenVals(parsedParameters, scope);
                }
            }
            catch (CommandLineException ex)
            {
                var errorMessage = $"ERROR: {ex.Message}";
                Console.WriteLine(errorMessage);
                Console.WriteLine(ex.StackTrace);
                Logger.Error(errorMessage);
                parser.ShowUsage();

                return 1;
            }
            catch (Exception ex)
            {
                var errorMessage = $"ERROR: {ex.Message}";
                Console.WriteLine(errorMessage);
                Console.WriteLine(ex.StackTrace);
                Logger.Error(errorMessage);
                return 1;
            }
        }

        /// <summary>
        /// Determines if the runner is running for generation or validation, 
        /// determined via parsed command arguments.
        /// </summary>
        /// <param name="parsedParameters"></param>
        private static void SetRunningMode(ArgumentParsingTarget parsedParameters)
        {
            if (parsedParameters.RegistrationFile != null)
            {
                _genValMode = GenValMode.Generate;
            }
            else
            {
                _genValMode = GenValMode.Validate;
            }
        }

        /// <summary>
        /// Configure logging for the app run.
        /// </summary>
        /// <param name="parsedParameters">The parsed arguments into the app</param>
        private static void ConfigureLogging(ArgumentParsingTarget parsedParameters)
        {
            string filePath;

            switch (_genValMode)
            {
                case GenValMode.Generate:
                    filePath = parsedParameters.RegistrationFile.FullName;
                    break;
                case GenValMode.Validate:
                    filePath = parsedParameters.AnswerFile.FullName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid {nameof(_genValMode)}");
            }

            string logName = $"{parsedParameters.Algorithm}-{parsedParameters.Mode}_{_genValMode}";

            LoggingHelper.ConfigureLogging(filePath, logName);
            Logger.Info($"{_genValMode} Test Vectors");
        }

        /// <summary>
        /// Allows input of arguments when not provided at invocation.
        /// Note this is not "exactly" the same as the entry, as the parsing it done by
        /// splitting on " ". Will not work as expected on(as example)
        /// files with spaces in the name
        /// 
        /// Using this shouldn't be the "normal flow" of application use.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        private static string[] GetArgsWhenNotProvided(string[] args, CommandLineParser.CommandLineParser parser)
        {
            if (args.Length != 0)
            {
                return args;
            }

            parser.ShowUsage();
            Console.WriteLine("cmd arguments were not provided, please provide them below:\n");
            var argsInput = Console.ReadLine();
            args = string.IsNullOrEmpty(argsInput) ? new string[0] : @argsInput.Split(' ');

            return args;
        }

        /// <summary>
        /// Run Generation or Validation, dependant on determined run mode..
        /// </summary>
        /// <param name="parsedParameters"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        private static int RunGenVals(ArgumentParsingTarget parsedParameters, IComponentContext scope)
        {
            if (_genValMode == GenValMode.Generate)
            {
                return RunGeneration(parsedParameters, scope);
            }

            return RunValidation(parsedParameters, scope);
        }

        /// <summary>
        /// Run Generation of test vectors for an algorithm.
        /// </summary>
        /// <param name="parsedParameters">The parsed command line arguments.</param>
        /// <param name="scope">The DI scope.</param>
        /// <returns></returns>
        private static int RunGeneration(ArgumentParsingTarget parsedParameters, IComponentContext scope)
        {
            var registrationFile = parsedParameters.RegistrationFile.FullName;

            var gen = scope.Resolve<IGenerator>();
            var result = gen.Generate(registrationFile);

            if (result.Success)
                return 0;

            var errorMessage = $"ERROR! Generating Test Vectors for {registrationFile}: {result.ErrorMessage}";
            Console.Error.WriteLine(errorMessage);
            Logger.Error(errorMessage);
            return 1;
        }

        /// <summary>
        /// Run Validation of test vectors for an algorithm.
        /// </summary>
        /// <param name="parsedParameters">The parsed command line arguments.</param>
        /// <param name="scope">The DI scope.</param>
        /// <returns></returns>
        private static int RunValidation(ArgumentParsingTarget parsedParameters, IComponentContext scope)
        {
            var responseFile = parsedParameters.ResponseFile.FullName;
            var answerFile = parsedParameters.AnswerFile.FullName;
            var promptFile = parsedParameters.PromptFile.FullName;

            var validator = scope.Resolve<IValidator>();
            var result = validator.Validate(responseFile, answerFile, promptFile);

            if (result.Success)
                return 0;

            var errorMessage = $"ERROR! Validating Test Vectors for {responseFile}: {result.ErrorMessage}";
            Console.Error.WriteLine(errorMessage);
            Logger.Error(errorMessage);
            return 1;
        }
    }
}

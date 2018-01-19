using CommandLineParser.Exceptions;
using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Generation.GenValApp.Models;
using NLog;

namespace NIST.CVP.Generation.GenValApp
{
    class Program
    {
        private const string _SETTINGS_FILE = "appSettings.json";
        private static readonly string DllDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly AlgorithmConfig Config;

        static Program()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            Config = new AlgorithmConfig();
            configuration.Bind(Config);
        }
        
        static int Main(string[] args)
        {
            var parser = new CommandLineParser.CommandLineParser();
            var parsedParameters = new ArgumentParsingTarget();
            parser.ExtractArgumentAttributes(parsedParameters);

            args = GetArgsWhenNotProvided(args, parser);

            try
            {
                parser.ParseCommandLine(args);

                var dllLocation = DllDirectory;
                if (parsedParameters.DllLocation != null)
                {
                    dllLocation = parsedParameters.DllLocation.FullName;
                }

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
                Logger.Error(errorMessage);
                parser.ShowUsage();

                return 1;
            }
            catch (Exception ex)
            {
                var errorMessage = $"ERROR: {ex.Message}";
                Console.WriteLine(errorMessage);
                Logger.Error(errorMessage);
                return 1;
            }
        }

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

        private static int RunGenVals(ArgumentParsingTarget parsedParameters, IComponentContext scope)
        {
            // Generation
            if (parsedParameters.RegistrationFile != null)
            {
                return RunGeneration(parsedParameters, scope);
            }

            // Validation
            return RunValidation(parsedParameters, scope);
        }

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

        private static Logger Logger => LogManager.GetLogger("GenValApp");
    }
}

﻿using CommandLineParser.Exceptions;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Generation.GenValApp.Models;
using NLog;

namespace NIST.CVP.Generation.GenValApp
{
    public static class Program
    {
        private static readonly AlgorithmConfig Config;

        public const string SETTINGS_FILE = "appSettings.json";
        public static readonly string RootDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string FileDirectory;

        /// <summary>
        /// Static constructor - bootstraps and sets configuration
        /// </summary>
        static Program()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"{RootDirectory}{SETTINGS_FILE}", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();

                var configuration = builder.Build();

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
                AutofacConfig.IoCConfiguration(Config, parsedParameters.Algorithm, parsedParameters.Mode, dllLocation);
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
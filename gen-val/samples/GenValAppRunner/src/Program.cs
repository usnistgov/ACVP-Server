using System;
using System.Threading.Tasks;
using CommandLineParser.Exceptions;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;
using NIST.CVP.ACVTS.Generation.GenValApp.Models;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Exceptions;
using NIST.CVP.ACVTS.Libraries.Generation;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle;
using NLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NIST.CVP.ACVTS.Generation.GenValApp
{
    public static class Program
    {
        static Program()
        {
            ServiceProvider = EntryPointConfigHelper.GetServiceProviderFromConfigurationBuilder();
        }

        private static IServiceProvider ServiceProvider { get; }

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
        public static async Task<int> Main(string[] args)
        {
            if (args.Length > 0) //Runs as original console application that reads file from directory 
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
                        return await genValRunner.Run(parsedParameters, runningOptions.GenValMode);
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
            else // sets up and runs controller for rest api
            {
              var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddScoped<IGeneratorResolver, GeneratorResolver>();
                builder.Services.AddScoped<IValidationResolver, ValidationResolver>();
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer(); // Enables Swagger endpoints
                builder.Services.AddSwaggerGen();           // Registers Swagger generator
                
                var app = builder.Build();

                // Enable Swagger unconditionally or based on an env variable
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "GenVal API V1");
                 c.RoutePrefix = "swagger"; // so it's hosted at /swagger
                });

                // Middleware setup
                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
                return 0;
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
                case GenValMode.Check:
                    filePath = parsedParameters.CapabilitiesFile.FullName;
                    break;
                case GenValMode.Generate:
                    filePath = parsedParameters.RegistrationFile.FullName;
                    break;
                case GenValMode.Validate:
                    filePath = parsedParameters.AnswerFile.FullName;
                    break;
                default:
                    filePath = string.Empty;
                    break;
            }

            var logName = $"{runningOptions.GenValMode}";

            LoggingHelper.ConfigureLogging(filePath, logName);
        }
    }
}

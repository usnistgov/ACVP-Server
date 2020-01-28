using Autofac;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.ParameterChecker.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Pools.Services;

namespace NIST.CVP.ParameterChecker.Helpers
{
    public class ParameterCheckRunner
    {
        private readonly IComponentContext _scope;
        protected IFileService FileService { get; set; } = new FileService();

        public ParameterCheckRunner(IComponentContext scope)
        {
            _scope = scope;
        }

        /// <summary>
        /// Configure logging for the app run.
        /// </summary>
        /// <param name="parsedParameters">The parsed arguments into the app</param>
        public void ConfigureLogging(ArgumentParsingTarget parsedParameters)
        {
            const string logName = "ParameterChecker";

            var filePath = parsedParameters.ParameterFile.FullName;

            LoggingHelper.ConfigureLogging(filePath, logName);
            Program.Logger.Info(logName);
        }

        /// <summary>
        /// Run Generation or Validation, dependent on determined run mode..
        /// </summary>
        /// <param name="parsedParameters"></param>
        /// <returns></returns>
        public int Run(ArgumentParsingTarget parsedParameters)
        {
            string errorMessage;
            try
            {
                var parameterFile = parsedParameters.ParameterFile.FullName;
                var outputDirPath = Path.GetDirectoryName(parameterFile);
                
                var result = RunParameterChecker(parameterFile);
                
                if (result.Success)
                    return (int)result.StatusCode;

                FileService.WriteFile(
                    JsonConvert.SerializeObject(
                        result,
                        new JsonSerializerSettings
                        {
                            Formatting = Formatting.Indented,
                            NullValueHandling = NullValueHandling.Ignore,
                            Converters = new JsonConverterProvider().GetJsonConverters(),
                        }
                    ),
                    Path.Combine(outputDirPath, "parameterCheck.json"),
                    true);
                
                errorMessage = $"ERROR! Checking Registration Parameters for {parameterFile}: {result.ErrorMessage}";
                Console.Error.WriteLine(errorMessage);
                Program.Logger.Error($"Status Code: {result.StatusCode}");
                Program.Logger.Error(errorMessage);

                return (int)result.StatusCode;
            }
            catch (Exception ex)
            {
                errorMessage = $"ERROR: {ex.Message}";
                Console.WriteLine(errorMessage);
                Console.WriteLine(ex.StackTrace);
                Logger.Error($"Status Code: {StatusCode.Exception}");
                Logger.Error(errorMessage);
                return (int)StatusCode.Exception;
            }
        }

        public ParameterCheckResponse RunParameterChecker(string registrationFile)
        {
            var paramChecker = _scope.Resolve<IParameterChecker>();

            try
            {
                return paramChecker.CheckParameters(new ParameterCheckRequest(FileService.ReadFile(registrationFile)));
            }
            catch (FileNotFoundException ex)
            {
                return new ParameterCheckResponse($"File {registrationFile} not found", StatusCode.FileReadError);
            }
        }

        public static ILogger Logger => LogManager.GetCurrentClassLogger();
    }
}

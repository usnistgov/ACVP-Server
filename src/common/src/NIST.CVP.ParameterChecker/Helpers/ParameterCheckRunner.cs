using System;
using System.IO;
using Autofac;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.ParameterChecker.Models;
using NLog;

namespace NIST.CVP.ParameterChecker.Helpers
{
    public class ParameterCheckRunner
    {
        private static string FileDirectory;
        private readonly IComponentContext _scope;

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
            var filePath = parsedParameters.ParameterFile.FullName;
            var logName = $"{parsedParameters.Algorithm}-{parsedParameters.Mode}";

            LoggingHelper.ConfigureLogging(filePath, logName);
            Program.Logger.Info($"ParameterChecker");
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
                FileDirectory = Path.GetDirectoryName(parsedParameters.ParameterFile.FullName);

                var parameterFile = parsedParameters.ParameterFile.FullName;
                var result = RunParameterChecker(parameterFile);

                if (result.Success)
                    return (int)result.StatusCode;

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
                return (int) StatusCode.Exception;
            }
        }

        public ParameterCheckResponse RunParameterChecker(string registrationFile)
        {
            var paramChecker = _scope.Resolve<IParameterChecker>();
            var result = paramChecker.CheckParameters(registrationFile);
            return result;
        }

        public static ILogger Logger => LogManager.GetCurrentClassLogger();
    }
}

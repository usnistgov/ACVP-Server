using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.GenValApp.Models;

namespace NIST.CVP.Generation.GenValApp.Helpers
{
    public class GenValRunner
    {
        public GenValMode GenValMode;
        private readonly IComponentContext _scope;

        public GenValRunner(IComponentContext scope)
        {
            _scope = scope;
        }

        /// <summary>
        /// Determines if the runner is running for generation or validation, 
        /// determined via parsed command arguments.
        /// </summary>
        /// <param name="parsedParameters"></param>
        public void SetRunningMode(ArgumentParsingTarget parsedParameters)
        {
            if (parsedParameters.RegistrationFile != null)
            {
                GenValMode = GenValMode.Generate;
            }
            else if (parsedParameters.AnswerFile != null && parsedParameters.ResponseFile != null)
            {
                GenValMode = GenValMode.Validate;
            }
            else
            {
                GenValMode = GenValMode.Unset;
            }
        }

        /// <summary>
        /// Configure logging for the app run.
        /// </summary>
        /// <param name="parsedParameters">The parsed arguments into the app</param>
        public void ConfigureLogging(ArgumentParsingTarget parsedParameters)
        {
            string filePath;

            switch (GenValMode)
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

            var logName = $"{parsedParameters.Algorithm}-{parsedParameters.Mode}_{GenValMode}";

            LoggingHelper.ConfigureLogging(filePath, logName);
            Program.Logger.Info($"{GenValMode} Test Vectors");
        }

        /// <summary>
        /// Run Generation or Validation, dependant on determined run mode..
        /// </summary>
        /// <param name="parsedParameters"></param>
        /// <returns></returns>
        public int Run(ArgumentParsingTarget parsedParameters)
        {
            string errorMessage;
            if (GenValMode == GenValMode.Generate)
            {
                var registrationFile = parsedParameters.RegistrationFile.FullName;
                var result = RunGeneration(registrationFile);

                if (result.Success)
                    return (int) result.StatusCode;

                errorMessage = $"ERROR! Generating Test Vectors for {registrationFile}: {result.ErrorMessage}";
                Console.Error.WriteLine(errorMessage);
                Program.Logger.Error($"Status Code: {result.StatusCode}");
                Program.Logger.Error(errorMessage);
                return (int) result.StatusCode;
            }
            else if (GenValMode == GenValMode.Validate)
            {
                var responseFile = parsedParameters.ResponseFile.FullName;
                var answerFile = parsedParameters.AnswerFile.FullName;
                var result = RunValidation(responseFile, answerFile);

                if (result.Success)
                    return (int) result.StatusCode;

                errorMessage = $"ERROR! Validating Test Vectors for {responseFile}: {result.ErrorMessage}";
                Console.Error.WriteLine(errorMessage);
                Program.Logger.Error($"Status Code: {result.StatusCode}");
                Program.Logger.Error(errorMessage);
                return (int) result.StatusCode;
            }

            errorMessage = "ERROR! Unable to find running mode";
            Console.Error.WriteLine(errorMessage);
            Program.Logger.Error($"Status Code: {StatusCode.ModeError}");
            Program.Logger.Error(errorMessage);
            return (int) StatusCode.ModeError;
        }

        /// <summary>
        /// Run Generation of test vectors for an algorithm.
        /// </summary>
        public GenerateResponse RunGeneration(string registrationFile)
        {
            var gen = _scope.Resolve<IGenerator>();
            var result = gen.Generate(registrationFile);
            return result;
        }

        /// <summary>
        /// Run Validation of test vectors for an algorithm.
        /// </summary>
        public ValidateResponse RunValidation(string responseFile, string answerFile)
        {
            var val = _scope.Resolve<IValidator>();
            var result = val.Validate(responseFile, answerFile);
            return result;
        }
    }
}

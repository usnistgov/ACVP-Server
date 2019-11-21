using Autofac;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.GenValApp.Models;
using NLog;
using System;
using System.IO;

namespace NIST.CVP.Generation.GenValApp.Helpers
{
    public class GenValRunner
    {
        private static string FileDirectory;
        private readonly IComponentContext _scope;
        protected IFileService FileService { get; set; } = new FileService();

        public GenValRunner(IComponentContext scope)
        {
            _scope = scope;
        }

        /// <summary>
        /// Run Generation or Validation, dependent on determined run mode..
        /// </summary>
        /// <param name="parsedParameters"></param>
        /// <returns></returns>
        public int Run(ArgumentParsingTarget parsedParameters, GenValMode genValMode)
        {
            string errorMessage;
            try
            {
                switch (genValMode)
                {
                    case GenValMode.Generate:
                        {
                            FileDirectory = Path.GetDirectoryName(parsedParameters.RegistrationFile.FullName);

                            var registrationFile = parsedParameters.RegistrationFile.FullName;
                            var result = RunGeneration(registrationFile);

                            var outputDirPath = Path.GetDirectoryName(registrationFile);
                            if (result.Success)
                            {
                                // Write out the produced vector files
                                FileService.WriteFile(
                                    Path.Combine(outputDirPath, "internalProjection.json"), 
                                    result.InternalProjection, 
                                    true);
                                FileService.WriteFile(
                                    Path.Combine(outputDirPath, "prompt.json"), 
                                    result.PromptProjection, 
                                    true);
                                FileService.WriteFile(
                                    Path.Combine(outputDirPath, "expectedResults.json"), 
                                    result.ResultProjection, 
                                    true);
                                
                                return (int)result.StatusCode;
                            }
                                
                            // Write out the error file
                            FileService.WriteFile(
                                Path.Combine(outputDirPath, "error.txt"),
                                result.ErrorMessage,
                                true);
                            
                            errorMessage = $"ERROR! Generating Test Vectors for {registrationFile}: {result.ErrorMessage}";
                            Console.Error.WriteLine(errorMessage);
                            Program.Logger.Error($"Status Code: {result.StatusCode}");
                            Program.Logger.Error(errorMessage);
                            ErrorLogger.LogError(result.StatusCode, "generator", result.ErrorMessage, FileDirectory);
                            return (int)result.StatusCode;
                        }

                    case GenValMode.Validate:
                        {
                            FileDirectory = Path.GetDirectoryName(parsedParameters.ResponseFile.FullName);

                            var responseFile = parsedParameters.ResponseFile.FullName;
                            var answerFile = parsedParameters.AnswerFile.FullName;
                            var showExpected = parsedParameters.ShowExpected;
                            var result = RunValidation(responseFile, answerFile, showExpected);

                            var outputDirPath = Path.GetDirectoryName(responseFile);
                            if (result.Success)
                            {
                                // Write out the response from the validation.
                                FileService.WriteFile(
                                    Path.Combine(outputDirPath, "validation.json"), 
                                    result.ValidationResult, 
                                    true);
                                
                                return (int)result.StatusCode;
                            }
                                
                            // Write out the error file
                            FileService.WriteFile(
                                Path.Combine(outputDirPath, "error.txt"),
                                result.ErrorMessage,
                                true);

                            errorMessage = $"ERROR! Validating Test Vectors for {responseFile}: {result.ErrorMessage}";
                            Console.Error.WriteLine(errorMessage);
                            Program.Logger.Error($"Status Code: {result.StatusCode}");
                            Program.Logger.Error(errorMessage);
                            ErrorLogger.LogError(result.StatusCode, "validator", result.ErrorMessage, FileDirectory);
                            return (int)result.StatusCode;
                        }

                    default:
                        errorMessage = "ERROR! Unable to find running mode";
                        Console.Error.WriteLine(errorMessage);
                        Program.Logger.Error($"Status Code: {StatusCode.ModeError}");
                        Program.Logger.Error(errorMessage);
                        return (int)StatusCode.ModeError;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"ERROR: {ex.Message}";
                Console.WriteLine(errorMessage);
                Console.WriteLine(ex.StackTrace);
                Logger.Error($"Status Code: {StatusCode.Exception}");
                Logger.Error(errorMessage);
                ErrorLogger.LogError(StatusCode.Exception, "driver", ex.Message, FileDirectory);
                return (int)StatusCode.Exception;
            }
        }

        /// <summary>
        /// Run Generation of test vectors for an algorithm.
        /// </summary>
        public GenerateResponse RunGeneration(string registrationFile)
        {
            var gen = _scope.Resolve<IGenerator>();
            
            var result = gen.Generate(new GenerateRequest(FileService.ReadFile(registrationFile)));
            return result;
        }

        /// <summary>
        /// Run Validation of test vectors for an algorithm.
        /// </summary>
        public ValidateResponse RunValidation(string responseFile, string answerFile, bool showExpected)
        {
            var val = _scope.Resolve<IValidator>();
            var result = val.Validate(new ValidateRequest(
                FileService.ReadFile(answerFile),
                FileService.ReadFile(responseFile),
                showExpected
                ));
            return result;
        }

        public static ILogger Logger => LogManager.GetCurrentClassLogger();
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using NIST.CVP.ACVTS.Generation.GenValApp.Models;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NLog;

namespace NIST.CVP.ACVTS.Generation.GenValApp.Helpers
{
    public class GenValRunner
    {
        private static string FileDirectory;
        private readonly IComponentContext _scope;

        public GenValRunner(IComponentContext scope)
        {
            _scope = scope;
        }

        protected IFileService FileService { get; set; } = new FileService();

        public static ILogger Logger => LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Run Generation or Validation, dependent on determined run mode..
        /// </summary>
        /// <param name="parsedParameters"></param>
        /// <returns></returns>
        public async Task<int> Run(ArgumentParsingTarget parsedParameters, GenValMode genValMode)
        {
            string errorMessage;
            try
            {
                switch (genValMode)
                {
                    case GenValMode.Check:
                    {
                        FileDirectory = Path.GetDirectoryName(parsedParameters.CapabilitiesFile.FullName);

                        var capabilitiesFile = parsedParameters.CapabilitiesFile.FullName;
                        var result = RunChecker(capabilitiesFile);

                        if (result.Success)
                        {
                            return (int)result.StatusCode;
                        }

                        var outputDirPath = Path.GetDirectoryName(capabilitiesFile);
                        var errorMsg = string.Join(", ", result.ErrorMessage);

                        // Write out the error file
                        FileService.WriteFile(
                            Path.Combine(outputDirPath, "error.txt"),
                            errorMsg,
                            true);

                        errorMessage = $"ERROR! Validating Capabilities for {capabilitiesFile}: {errorMsg}";
                        Console.Error.WriteLine(errorMessage);
                        Program.Logger.Error($"Status Code: {result.StatusCode}");
                        Program.Logger.Error(errorMessage);
                        ErrorLogger.LogError(result.StatusCode, "checker", errorMessage, FileDirectory);
                        return (int)result.StatusCode;
                    }
                    case GenValMode.Generate:
                        {
                            FileDirectory = Path.GetDirectoryName(parsedParameters.RegistrationFile.FullName);

                            var registrationFile = parsedParameters.RegistrationFile.FullName;
                            var result = await RunGeneration(registrationFile);

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
                            var result = await RunValidation(responseFile, answerFile, showExpected);

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
        /// Run Validation of the algorithm capabilies.
        /// </summary>
        public ParameterCheckResponse RunChecker(string capabilitiesFile)
        {
            var checker = _scope.Resolve<IParameterChecker>();
            var result = checker.CheckParameters(new ParameterCheckRequest(FileService.ReadFile(capabilitiesFile)));
            return result;
        }
        
        /// <summary>
        /// Run Generation of test vectors for an algorithm.
        /// </summary>
        public Task<GenerateResponse> RunGeneration(string registrationFile)
        {
            var gen = _scope.Resolve<IGenerator>();
            
            var result = gen.GenerateAsync(new GenerateRequest(FileService.ReadFile(registrationFile)));
            return result;
        }

        /// <summary>
        /// Run Validation of test vectors for an algorithm.
        /// </summary>
        public Task<ValidateResponse> RunValidation(string responseFile, string answerFile, bool showExpected)
        {
            var val = _scope.Resolve<IValidator>();
            var result = val.ValidateAsync(new ValidateRequest(
                FileService.ReadFile(answerFile),
                FileService.ReadFile(responseFile),
                showExpected
                ));
            return result;
        }
    }
}

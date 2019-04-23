using System;
using System.Collections.Generic;
using System.IO;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Parsers;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public class ParameterChecker<TParameters> : IParameterChecker
        where TParameters : IParameters
    {
        private readonly IParameterParser<TParameters> _parameterParser;
        private readonly IParameterValidator<TParameters> _parameterValidator;

        public ParameterChecker(IParameterParser<TParameters> parameterParser, IParameterValidator<TParameters> parameterValidator)
        {
            _parameterParser = parameterParser;
            _parameterValidator = parameterValidator;
        }

        public ParameterCheckResponse CheckParameters(string registrationFile)
        {
            try
            {
                var parameterResponse = _parameterParser.Parse(registrationFile);
                if (!parameterResponse.Success)
                {
                    return new ParameterCheckResponse(parameterResponse.ErrorMessage, StatusCode.ParameterError);
                }

                var parameters = parameterResponse.ParsedObject;
                var validateResponse = _parameterValidator.Validate(parameters);
                if (!validateResponse.Success)
                {
                    return new ParameterCheckResponse(validateResponse.ErrorMessage, StatusCode.ParameterValidationError);
                }

                return SaveOutputs(requestFilePath, testVector);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new ParameterCheckResponse("General exception. Contact service provider.", StatusCode.Exception);
            }
        }

        protected ParameterCheckResponse SaveOutputs(string requestFilePath, TTestVectorSet testVector)
        {
            var outputDirPath = Path.GetDirectoryName(requestFilePath);
            var saveResult = SaveProjectionToFile(outputDirPath, testVector, jsonOutput);
            if (!string.IsNullOrEmpty(saveResult))
            {
                return new ParameterCheckResponse(saveResult, StatusCode.FileSaveError);
            }

            return new ParameterCheckResponse();
        }

        private string SaveProjectionToFile(string outputPath, TTestVectorSet testVectorSet, JsonOutputDetail jsonOutput)
        {
            var json = _vectorSetSerializer.Serialize(testVectorSet, jsonOutput.Projection);

            return SaveToFile(outputPath, jsonOutput.FileName, json);
        }

        private string SaveToFile(string fileRoot, string fileName, string json)
        {
            string path = Path.Combine(fileRoot, fileName);
            try
            {
                File.WriteAllText(path, json);
                return null;
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return $"Could not create {path}";
            }
        }

        protected Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

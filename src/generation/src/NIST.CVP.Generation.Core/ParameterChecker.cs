using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.Core.Parsers;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public class ParameterChecker<TParameters> : IParameterChecker
        where TParameters : IParameters
    {
        private readonly IParameterParser<TParameters> _parameterParser;
        private readonly IParameterValidator<TParameters> _parameterValidator;
        private readonly IList<JsonConverter> _jsonConverters = new JsonConverterProvider().GetJsonConverters().ToList();

        public ParameterChecker(IParameterParser<TParameters> parameterParser, IParameterValidator<TParameters> parameterValidator)
        {
            _parameterParser = parameterParser;
            _parameterValidator = parameterValidator;
        }

        public ParameterCheckResponse CheckParameters(ParameterCheckRequest request)
        {
            try
            {
                var parameterResponse = _parameterParser.Parse(request.RegistrationJson);
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

                return new ParameterCheckResponse();
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new ParameterCheckResponse("General exception. Contact service provider.", StatusCode.Exception);
            }
        }

        protected ParameterCheckResponse SaveOutputs(string requestFilePath, ParameterCheckResponse response)
        {
            var outputDirPath = Path.GetDirectoryName(requestFilePath);
            var json = JsonConvert.SerializeObject(
                response,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = _jsonConverters,
                }
            );

            var saveResult = SaveToFile(outputDirPath, "parameterCheck.json", json);
            if (!string.IsNullOrEmpty(saveResult))
            {
                return new ParameterCheckResponse(saveResult, StatusCode.FileSaveError);
            }

            return response;
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

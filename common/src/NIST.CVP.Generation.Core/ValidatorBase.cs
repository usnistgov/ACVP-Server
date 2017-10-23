using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.Core.Resolvers;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public abstract class ValidatorBase
    {
        protected IDynamicParser _dynamicParser;

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }

        public ValidateResponse Validate(string resultPath, string answerPath, string promptPath)
        {
            var answerParseResponse = _dynamicParser.Parse(answerPath);
            if (!answerParseResponse.Success)
            {
                return new ValidateResponse(answerParseResponse.ErrorMessage);
            }
            var promptParseResponse = _dynamicParser.Parse(promptPath);
            if (!promptParseResponse.Success)
            {
                return new ValidateResponse(promptParseResponse.ErrorMessage);
            }

            var testResultParseResponse = _dynamicParser.Parse(resultPath);
            if (!testResultParseResponse.Success)
            {
                return new ValidateResponse(testResultParseResponse.ErrorMessage);
            }

            var response = ValidateWorker(answerParseResponse, promptParseResponse, testResultParseResponse);

            var validationJson = JsonConvert.SerializeObject(response, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new ValidationResolver(),
                    //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });
            var saveResult = SaveToFile(Path.GetDirectoryName(resultPath), "validation.json", validationJson);
            if (!string.IsNullOrEmpty(saveResult))
            {
                return new ValidateResponse(saveResult);
            }

            return new ValidateResponse();
        }

        public abstract TestVectorValidation ValidateWorker(ParseResponse<object> answerParseResponse, ParseResponse<object> promptParseResponse, ParseResponse<object> testResultParseResponse);

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
    }
}
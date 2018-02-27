using System;
using System.IO;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.Core.Resolvers;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public class Validator<TTestVectorSet, TTestGroup, TTestCase> : IValidator
        where TTestVectorSet : ITestVectorSet
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        protected IDynamicParser _dynamicParser;
        protected readonly IResultValidator<TTestGroup, TTestCase> _resultValidator;
        protected readonly ITestCaseValidatorFactory<TTestVectorSet, TTestCase> _testCaseValidatorFactory;
        protected readonly ITestReconstitutor<TTestVectorSet, TTestGroup> _testReconstitutor;

        public Validator(IDynamicParser dynamicParser, IResultValidator<TTestGroup, TTestCase> resultValidator, ITestCaseValidatorFactory<TTestVectorSet, TTestCase> testCaseValidatorFactory, ITestReconstitutor<TTestVectorSet, TTestGroup> testReconstitutor)
        {
            _dynamicParser = dynamicParser;
            _resultValidator = resultValidator;
            _testCaseValidatorFactory = testCaseValidatorFactory;
            _testReconstitutor = testReconstitutor;
        }

        public ValidateResponse Validate(string resultPath, string answerPath)
        {
            var answerParseResponse = _dynamicParser.Parse(answerPath);
            if (!answerParseResponse.Success)
            {
                return new ValidateResponse(answerParseResponse.ErrorMessage);
            }

            var testResultParseResponse = _dynamicParser.Parse(resultPath);
            if (!testResultParseResponse.Success)
            {
                return new ValidateResponse(testResultParseResponse.ErrorMessage);
            }

            var response = ValidateWorker(answerParseResponse, testResultParseResponse);

            var validationJson = JsonConvert.SerializeObject(response, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new ValidationResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });
            var saveResult = SaveToFile(Path.GetDirectoryName(resultPath), "validation.json", validationJson);
            if (!string.IsNullOrEmpty(saveResult))
            {
                return new ValidateResponse(saveResult);
            }

            return new ValidateResponse();
        }

        protected virtual TestVectorValidation ValidateWorker(ParseResponse<dynamic> answerParseResponse, ParseResponse<dynamic> testResultParseResponse)
        {
            var testVectorSet = _testReconstitutor.GetTestVectorSetExpectationFromResponse(answerParseResponse.ParsedObject);
            var results = testResultParseResponse.ParsedObject;
            var suppliedResults = _testReconstitutor.GetTestGroupsFromResultResponse(results.testResults);
            var testCaseValidators = _testCaseValidatorFactory.GetValidators(testVectorSet);
            var response = _resultValidator.ValidateResults(testCaseValidators, suppliedResults);
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

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.AES
{
    public class Validator<TTestVectorSet, TTestCase> : ValidatorBase
        where TTestVectorSet : ITestVectorSet
        where TTestCase : ITestCase
    {
        private readonly IResultValidator<TTestCase> _resultValidator;
        private readonly ITestCaseValidatorFactory<TTestVectorSet, TTestCase> _testCaseValidatorFactory;
        private readonly ITestReconstitutor<TTestVectorSet, TTestCase> _testReconstitutor;

        public Validator(IDynamicParser dynamicParser, IResultValidator<TTestCase> resultValidator, ITestCaseValidatorFactory<TTestVectorSet, TTestCase> testCaseValidatorFactory, ITestReconstitutor<TTestVectorSet, TTestCase> testReconstitutor)
        {
            _dynamicParser = dynamicParser;
            _resultValidator = resultValidator;
            _testCaseValidatorFactory = testCaseValidatorFactory;
            _testReconstitutor = testReconstitutor;
        }

        public override TestVectorValidation ValidateWorker(ParseResponse<dynamic> answerParseResponse, ParseResponse<dynamic> promptParseResponse, ParseResponse<dynamic> testResultParseResponse)
        {
            var testVectorSet = _testReconstitutor
                .GetTestVectorSetExpectationFromResponse(answerParseResponse.ParsedObject, promptParseResponse.ParsedObject);
            var results = testResultParseResponse.ParsedObject;
            var suppliedResults = _testReconstitutor.GetTestCasesFromResultResponse(results.testResults);
            var testCases = _testCaseValidatorFactory.GetValidators(testVectorSet, suppliedResults);
            var response = _resultValidator.ValidateResults(testCases, suppliedResults);
            return response;
        }
    }
}

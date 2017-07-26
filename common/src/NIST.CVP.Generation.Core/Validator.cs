using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.Core
{
    public class Validator<TTestVectorSet, TTestCase> : ValidatorBase, IValidator
        where TTestVectorSet : ITestVectorSet
        where TTestCase : ITestCase
    {
        protected readonly IResultValidator<TTestCase> _resultValidator;
        protected readonly ITestCaseValidatorFactory<TTestVectorSet, TTestCase> _testCaseValidatorFactory;
        protected readonly ITestReconstitutor<TTestVectorSet, TTestCase> _testReconstitutor;

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

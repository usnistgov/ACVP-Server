using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.SHA3
{
    public class Validator : ValidatorBase
    {
        private readonly IResultValidator<TestCase> _resultValidator;
        private readonly ITestCaseValidatorFactory<TestVectorSet, TestCase> _testCaseValidatorFactory;

        public Validator(IDynamicParser dynamicParser, IResultValidator<TestCase> resultValidator,
            ITestCaseValidatorFactory<TestVectorSet, TestCase> testCaseValidatorFactory)
        {
            _dynamicParser = dynamicParser;
            _resultValidator = resultValidator;
            _testCaseValidatorFactory = testCaseValidatorFactory;
        }

        public override TestVectorValidation ValidateWorker(ParseResponse<dynamic> answerParseResponse,
            ParseResponse<dynamic> promptParseResponse, ParseResponse<dynamic> testResultParseResponse)
        {
            var testVectorSet = new TestVectorSet(answerParseResponse.ParsedObject, promptParseResponse.ParsedObject);
            var results = testResultParseResponse.ParsedObject;
            var suppliedResults = GetTestCaseResults(results.testResults);
            var testCases = _testCaseValidatorFactory.GetValidators(testVectorSet, suppliedResults);
            var response = _resultValidator.ValidateResults(testCases, suppliedResults);

            return response;
        }

        private List<TestCase> GetTestCaseResults(dynamic results)
        {
            var list = new List<TestCase>();
            foreach (var result in results)
            {
                list.Add(new TestCase(result));
            }
            return list;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.SHA1
{
    public class Validator : ValidatorBase
    {
        private readonly IResultValidator<TestCase> _resultValidator;
        private readonly ITestCaseGeneratorFactory _testCaseGeneratorFactory;

        public Validator(IDynamicParser dynamicParser, IResultValidator<TestCase> resultValidator,
            ITestCaseGeneratorFactory testCaseGeneratorFactory)
        {
            _dynamicParser = dynamicParser;
            _resultValidator = resultValidator;
            _testCaseGeneratorFactory = testCaseGeneratorFactory;
        }

        public override TestVectorValidation ValidateWorker(ParseResponse<dynamic> answerParseResponse, ParseResponse<dynamic> promptParseResponse,
            ParseResponse<dynamic> testResultParseResponse)
        {
            var testVectorSet = new TestVectorSet(answerParseResponse.ParsedObject, promptParseResponse.ParsedObject);
            var results = testResultParseResponse.ParsedObject;
            var suppliedResults = GetTestCaseResults(results.testResults);
            var testCases = BuildValidatorList(testVectorSet, suppliedResults);
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

        private List<ITestCaseValidator<TestCase>> BuildValidatorList(TestVectorSet testVectorSet,
            List<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup) g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator();
                foreach (var test in group.Tests.Select(t => (TestCase) t))
                {
                    var workingTest = test;
                    if (test.Deferred)
                    {
                        var matchingResult = suppliedResults.FirstOrDefault(r => r.TestCaseId == test.TestCaseId);
                        var protoTest = new TestCase
                        {
                            Message = test.Message,
                            Digest = test.Digest
                        };

                        var genResult = generator.Generate(group, protoTest);
                        if (!genResult.Success)
                        {
                            throw new Exception($"Could not generate results for testCase = {test.TestCaseId}");
                        }
                    }

                    list.Add(new TestCaseValidatorHash(workingTest));
                }
            }

            return list;
        }
    }
}

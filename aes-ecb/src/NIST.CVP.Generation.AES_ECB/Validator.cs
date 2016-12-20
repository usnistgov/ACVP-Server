using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_ECB
{
    public class Validator : ValidatorBase
    {
        private readonly IResultValidator _resultValidator;

        public Validator(IDynamicParser dynamicParser, IResultValidator resultValidator)
        {
            _dynamicParser = dynamicParser;
            _resultValidator = resultValidator;
        }

        public override TestVectorValidation ValidateWorker(ParseResponse<dynamic> answerParseResponse, ParseResponse<dynamic> promptParseResponse, ParseResponse<dynamic> testResultParseResponse)
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

      

        private List<ITestCaseValidator> BuildValidatorList(TestVectorSet testVectorSet, List<TestCase>  suppliedResults)
        {

            var list = new List<ITestCaseValidator>();
           
            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    var workingTest = test;
                    if (group.Function == "encrypt")
                    {
                        list.Add(new TestCaseValidatorEncrypt(workingTest));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorDecrypt(workingTest));
                    }
                }
            }

            return list;
        }
    }
}

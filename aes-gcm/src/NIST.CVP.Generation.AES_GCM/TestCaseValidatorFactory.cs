using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly ITestCaseGeneratorFactory<TestGroup, TestCase> _testCaseGeneratorFactory;

        public TestCaseValidatorFactory(ITestCaseGeneratorFactory<TestGroup, TestCase> testCaseGeneratorFactory)
        {
            _testCaseGeneratorFactory = testCaseGeneratorFactory;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group);
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    var workingTest = test;
                    if (test.Deferred)
                    {
                        //if we're waiting for additional input on the response...
                        var matchingResult = suppliedResults.FirstOrDefault(r => r.TestCaseId == test.TestCaseId);
                        var protoTest = new TestCase
                        {
                            AAD = test.AAD,
                            Key = test.Key,
                            PlainText = test.PlainText,
                            CipherText = test.CipherText,
                            Tag = test.Tag,
                            IV = matchingResult.IV
                        };
                        var genResult = generator.Generate(group, protoTest);
                        if (!genResult.Success)
                        {
                            throw new Exception($"Could not generate results. for testCase = {test.TestCaseId}");
                        }
                    }
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

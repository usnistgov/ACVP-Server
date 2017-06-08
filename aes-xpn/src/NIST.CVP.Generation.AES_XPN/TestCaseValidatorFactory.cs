using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XPN
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
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    var workingTest = test;

                    if (group.Function == "encrypt")
                    {
                        if (workingTest.Deferred)
                        {
                            list.Add(new TestCaseValidatorInternalEncrypt(workingTest, group, _testCaseGeneratorFactory));
                        }
                        else
                        {
                            list.Add(new TestCaseValidatorExternalEncrypt(workingTest));
                        }
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

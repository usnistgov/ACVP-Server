using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseValidatorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups)
            {
                foreach (var test in group.Tests)
                {
                    var testType = group.TestType.ToLower();

                    if (testType == "aft" || testType == "gdt")
                    {
                        list.Add(new TestCaseValidatorAft(test, group, new DeferredTestCaseResolver(_oracle)));
                    }
                    else if (testType == "kat")
                    {
                        list.Add(new TestCaseValidatorKat(test));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorNull("Could not determine TestType for TestCase", test.TestCaseId));
                    }
                }
            }

            return list;
        }
    }
}


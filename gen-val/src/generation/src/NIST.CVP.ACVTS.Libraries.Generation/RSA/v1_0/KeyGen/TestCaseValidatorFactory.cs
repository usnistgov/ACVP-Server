using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseValidatorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups)
            {
                foreach (var test in group.Tests)
                {
                    var testType = group.TestType.ToLower();

                    if (testType == "aft")
                    {
                        list.Add(new TestCaseValidatorAft(test, group, new DeferredTestCaseResolverAFT(_oracle)));
                    }
                    else if (testType == "gdt")
                    {
                        list.Add(new TestCaseValidatorAft(test, group, new DeferredTestCaseResolverGDT(_oracle)));
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


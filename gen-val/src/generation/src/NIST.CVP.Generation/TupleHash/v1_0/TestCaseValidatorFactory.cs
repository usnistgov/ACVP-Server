using System.Collections.Generic;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.TupleHash.v1_0
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        public IEnumerable<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups)
            {
                foreach (var test in group.Tests)
                {
                    var workingTest = test;
                    if (group.TestType.ToLower() == "mct")
                    {
                        list.Add(new TestCaseValidatorMCTHash(workingTest));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorHash(workingTest));
                    }
                }
            }

            return list;
        }
    }
}

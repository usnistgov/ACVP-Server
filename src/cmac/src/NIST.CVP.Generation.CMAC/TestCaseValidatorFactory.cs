using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        public IEnumerable<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
                {
                    var workingTest = test;
                    if (group.Function.ToLower() == "gen")
                    {
                        list.Add(new TestCaseValidatorGen(workingTest));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorVer(workingTest));
                    }
                }
            }

            return list;
        }
    }
}

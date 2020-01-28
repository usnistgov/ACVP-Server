using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.v1_0.PqgVer
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
                    if (group.TestType.ToLower() == "gdt")
                    {
                        list.Add(new TestCaseValidator(test));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorNull("Could not find TestType for group", test.TestCaseId));
                    }
                }
            }

            return list;
        }
    }
}

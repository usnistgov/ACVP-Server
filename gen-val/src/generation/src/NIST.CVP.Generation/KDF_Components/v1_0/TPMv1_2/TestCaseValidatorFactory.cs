using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KDF_Components.v1_0.TPMv1_2
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
                {
                    list.Add(new TestCaseValidator(test));
                }
            }

            return list;
        }
    }
}

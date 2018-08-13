using System.Collections.Generic;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        public IEnumerable<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

            foreach(var group in testVectorSet.TestGroups)
            {
                foreach(var test in group.Tests)
                {
                    list.Add(new TestCaseValidator(test));
                }
            }

            return list;
        }
    }
}

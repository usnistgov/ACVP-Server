using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

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

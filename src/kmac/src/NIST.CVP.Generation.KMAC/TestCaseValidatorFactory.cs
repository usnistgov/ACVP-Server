using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            throw new System.NotImplementedException();
        }
    }
}

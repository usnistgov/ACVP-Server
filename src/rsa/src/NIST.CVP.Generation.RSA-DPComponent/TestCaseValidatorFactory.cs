using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IRsa _rsa;

        public TestCaseValidatorFactory(IRsa rsa)
        {
            _rsa = rsa;
        }

        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups)
            {
                foreach (var test in group.Tests)
                {
                    list.Add(new TestCaseValidator(group, test, new DeferredCryptoResolver(_rsa)));
                }
            }

            return list;
        }
    }
}

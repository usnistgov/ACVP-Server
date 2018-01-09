using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, SharedSecretResponse> _deferredTestCaseResolver;

        public TestCaseValidatorFactory(IDeferredTestCaseResolver<TestGroup, TestCase, SharedSecretResponse> deferredTestCaseResolver)
        {
            _deferredTestCaseResolver = deferredTestCaseResolver;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    var workingTest = test;
                    list.Add(new TestCaseValidator(workingTest, group, _deferredTestCaseResolver));
                }
            }

            return list;
        }
    }
}
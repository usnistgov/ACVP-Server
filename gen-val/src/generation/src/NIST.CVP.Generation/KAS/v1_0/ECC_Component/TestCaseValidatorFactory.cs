using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KAS.v1_0.ECC_Component
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, SharedSecretResponse> _deferredTestCaseResolver;

        public TestCaseValidatorFactory(IDeferredTestCaseResolverAsync<TestGroup, TestCase, SharedSecretResponse> deferredTestCaseResolver)
        {
            _deferredTestCaseResolver = deferredTestCaseResolver;
        }

        public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
                {
                    var workingTest = test;
                    list.Add(new TestCaseValidator(workingTest, group, _deferredTestCaseResolver));
                }
            }

            return list;
        }
    }
}
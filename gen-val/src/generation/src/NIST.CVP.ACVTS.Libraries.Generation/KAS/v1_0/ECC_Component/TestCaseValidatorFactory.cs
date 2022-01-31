using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component
{
    public class TestCaseValidatorFactory<TTestVectorSet> : ITestCaseValidatorFactoryAsync<TTestVectorSet, TestGroup, TestCase>
        where TTestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, SharedSecretResponse> _deferredTestCaseResolver;

        public TestCaseValidatorFactory(IDeferredTestCaseResolverAsync<TestGroup, TestCase, SharedSecretResponse> deferredTestCaseResolver)
        {
            _deferredTestCaseResolver = deferredTestCaseResolver;
        }

        public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TTestVectorSet testVectorSet)
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

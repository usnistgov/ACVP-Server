using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, SharedSecretResponse> _deferredTestCaseResolver;

        public TestCaseValidatorFactory(IDeferredTestCaseResolver<TestGroup, TestCase, SharedSecretResponse> deferredTestCaseResolver)
        {
            _deferredTestCaseResolver = deferredTestCaseResolver;
        }

        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

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
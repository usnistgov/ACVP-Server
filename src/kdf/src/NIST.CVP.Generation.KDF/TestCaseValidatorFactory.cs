using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IKdfFactory _factory;

        public TestCaseValidatorFactory(IKdfFactory factory)
        {
            _factory = factory;
        }

        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();
            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                var algo = _factory.GetKdfInstance(group.KdfMode, group.MacMode, group.CounterLocation, group.CounterLength);

                foreach (var test in group.Tests.Select(t => t))
                {
                    list.Add(new TestCaseValidator(test, group, new DeferredTestCaseResolver(algo)));
                }
            }

            return list;
        }
    }
}

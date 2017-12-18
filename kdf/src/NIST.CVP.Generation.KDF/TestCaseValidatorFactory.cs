using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly IKdfFactory _factory;

        public TestCaseValidatorFactory(IKdfFactory factory)
        {
            _factory = factory;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();
            var kdfFactory = new KdfFactory();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup) g))
            {
                var algo = kdfFactory.GetKdfInstance(group.KdfMode, group.MacMode, group.CounterLocation, group.CounterLength);

                foreach (var test in group.Tests.Select(t => (TestCase) t))
                {
                    list.Add(new TestCaseValidator(test, group, new DeferredTestCaseResolver(algo)));
                }
            }

            return list;
        }
    }
}

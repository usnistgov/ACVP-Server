using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseValidatorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
                {
                    var deferredResolver = new DeferredTestCaseResolver(_oracle);
                    list.Add(new TestCaseValidator(test, group, deferredResolver));
                }
            }

            return list;
        }
    }
}

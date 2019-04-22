using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.v1_0.PqgGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseValidatorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public IEnumerable<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
                {
                    if (group.PQGenMode != PrimeGenMode.None)
                    {
                        var deferredResolver = new DeferredTestCaseResolverPQ(_oracle);
                        list.Add(new TestCaseValidatorPQ(test, group, deferredResolver));
                    }
                    else if (group.GGenMode != GeneratorGenMode.None)
                    {
                        var deferredResolver = new DeferredTestCaseResolverG(_oracle);
                        list.Add(new TestCaseValidatorG(test, group, deferredResolver));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorNull("Could not find validator for group", test.TestCaseId));
                    }
                }
            }

            return list;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseValidatorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
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

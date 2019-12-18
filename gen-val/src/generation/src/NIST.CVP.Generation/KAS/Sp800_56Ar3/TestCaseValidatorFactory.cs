using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3
{
    public class TestCaseValidatorFactory<TTestVectorSet, TTestGroup, TTestCase, TKeyPair> : ITestCaseValidatorFactoryAsync<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : TestVectorSetBase<TTestGroup, TTestCase, TKeyPair>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TKeyPair : IDsaKeyPair
    {
        private readonly IOracle _oracle;

        public TestCaseValidatorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public IEnumerable<ITestCaseValidatorAsync<TTestGroup, TTestCase>> GetValidators(TTestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TTestGroup, TTestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
                {
                    var workingTest = test;

                    if (group.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(new TestCaseValidatorAft<TTestGroup, TTestCase, TKeyPair>(
                            workingTest,
                            group,
                            new DeferredTestCaseResolver<TTestGroup, TTestCase, TKeyPair>(_oracle)));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorVal<TTestGroup, TTestCase, TKeyPair>(workingTest));
                    }
                }
            }

            return list;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly IShaFactory _shaFactory;
        private readonly IPQGeneratorValidatorFactory _pqGenFactory;
        private readonly IGGeneratorValidatorFactory _gGenFactory;

        public TestCaseValidatorFactory(IShaFactory shaFactory, IPQGeneratorValidatorFactory pqGenFactory, IGGeneratorValidatorFactory gGenFactory)
        {
            _shaFactory = shaFactory;
            _pqGenFactory = pqGenFactory;
            _gGenFactory = gGenFactory;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    if (group.PQGenMode != PrimeGenMode.None)
                    {
                        var deferredResolver = new DeferredTestCaseResolverPQ(_pqGenFactory, _shaFactory);
                        list.Add(new TestCaseValidatorPQ(test, group, deferredResolver));
                    }
                    else if (group.GGenMode != GeneratorGenMode.None)
                    {
                        var deferredResolver = new DeferredTestCaseResolverG(_gGenFactory, _shaFactory);
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

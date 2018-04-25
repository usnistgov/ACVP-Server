using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IDsaEccFactory _eccDsaFactory;
        private readonly IEccCurveFactory _curveFactory;

        public TestCaseValidatorFactory(IDsaEccFactory eccDsaFactory, IEccCurveFactory curveFactory)
        {
            _eccDsaFactory = eccDsaFactory;
            _curveFactory = curveFactory;
        }

        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
                {
                    var deferredResolver = new DeferredTestCaseResolver(_eccDsaFactory, _curveFactory);
                    list.Add(new TestCaseValidator(test, group, deferredResolver));
                }
            }

            return list;
        }
    }
}

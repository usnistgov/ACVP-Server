using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly IDsaEcc _eccDsa;
        private readonly IEccCurveFactory _curveFactory;

        public TestCaseValidatorFactory(IDsaEcc eccDsa, IEccCurveFactory curveFactory)
        {
            _eccDsa = eccDsa;
            _curveFactory = curveFactory;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    list.Add(new TestCaseValidator(test, group, _eccDsa, _curveFactory));
                }
            }

            return list;
        }
    }
}

using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.SigVer
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        private readonly IDsaEccFactory _eccDsaFactory;
        private readonly IEccCurveFactory _curveFactory;

        public TestGroupGeneratorFactory(IDsaEccFactory eccDsaFactory, IEccCurveFactory curveFactory)
        {
            _eccDsaFactory = eccDsaFactory;
            _curveFactory = curveFactory;
        }

        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGenerator(_eccDsaFactory, _curveFactory)
            };

            return list;
        }
    }
}

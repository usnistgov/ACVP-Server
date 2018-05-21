using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IDsaEccFactory _dsaFactory;
        private readonly IEccDhComponent _eccDhComponent;

        public TestCaseGeneratorFactory(IEccCurveFactory curveFactory, IDsaEccFactory dsaFactory, IEccDhComponent eccDhComponent)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
            _eccDhComponent = eccDhComponent;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var curve = _curveFactory.GetCurve(testGroup.Curve);
            // note hash function is used for signing/verifying - not relevant for use in this algo
            var dsa = _dsaFactory.GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512));

            return new TestCaseGenerator(curve, dsa, _eccDhComponent);
        }
    }
}
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IShaFactory _shaFactory;
        private readonly IDsaEcc _eccDsa;
        private readonly IEccCurveFactory _curveFactory;

        public TestCaseGeneratorFactory(IRandom800_90 rand, IShaFactory shaFactory, IDsaEcc eccDsa, IEccCurveFactory curveFactory)
        {
            _random800_90 = rand;
            _shaFactory = shaFactory;
            _eccDsa = eccDsa;
            _curveFactory = curveFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            return new TestCaseGenerator(_random800_90, _eccDsa, _shaFactory, _curveFactory);
        }
    }
}

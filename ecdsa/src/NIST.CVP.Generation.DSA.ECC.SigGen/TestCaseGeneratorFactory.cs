using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IShaFactory _shaFactory;
        private IDsaEcc _eccDsa;

        public TestCaseGeneratorFactory(IRandom800_90 rand)
        {
            _random800_90 = rand;
            _shaFactory = new ShaFactory();
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            _eccDsa = new EccDsa(_shaFactory.GetShaInstance(testGroup.HashAlg));
            return new TestCaseGenerator(_random800_90, _eccDsa);
        }
    }
}

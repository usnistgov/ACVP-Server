using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISignatureBuilder _signatureBuilder;
        private readonly IPaddingFactory _paddingFactory;
        private readonly IShaFactory _shaFactory;
        private readonly IRsa _rsa;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISignatureBuilder sigBuilder, IPaddingFactory padFactory, IShaFactory shaFactory, IRsa rsa)
        {
            _random800_90 = random800_90;
            _signatureBuilder = sigBuilder;
            _paddingFactory = padFactory;
            _shaFactory = shaFactory;
            _rsa = rsa;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            if (testGroup.TestType.ToLower() == "gdt")
            {
                return new TestCaseGeneratorGDT(_random800_90, _signatureBuilder, _paddingFactory, _shaFactory, _rsa);
            }

            return new TestCaseGeneratorNull();
        }
    }
}

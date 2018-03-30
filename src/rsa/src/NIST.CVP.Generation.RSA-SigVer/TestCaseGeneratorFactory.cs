using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Signatures;
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

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISignatureBuilder sigBuilder, IPaddingFactory padFactory, IShaFactory shaFactory)
        {
            _random800_90 = random800_90;
            _signatureBuilder = sigBuilder;
            _paddingFactory = padFactory;
            _shaFactory = shaFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            if (testGroup.TestType.ToLower() == "gdt")
            {
                return new TestCaseGeneratorGDT(_random800_90, _signatureBuilder, _paddingFactory, _shaFactory);
            }

            return new TestCaseGeneratorNull();
        }
    }
}

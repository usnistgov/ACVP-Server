using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.Signatures;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISignatureBuilder _signatureBuilder;
        private readonly IPaddingFactory _paddingFactory;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IShaFactory _shaFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISignatureBuilder sigBuilder,
            IPaddingFactory paddingFactory, IKeyBuilder keyBuilder, IKeyComposerFactory keyComposerFactory,
            IShaFactory shaFactory)
        {
            _random800_90 = random800_90;
            _signatureBuilder = sigBuilder;
            _paddingFactory = paddingFactory;
            _keyBuilder = keyBuilder;
            _keyComposerFactory = keyComposerFactory;
            _shaFactory = shaFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            if (testGroup.TestType.ToLower() == "gdt")
            {
                return new TestCaseGeneratorGDT(_random800_90, _signatureBuilder, _keyBuilder, _paddingFactory, _shaFactory, _keyComposerFactory);
            }

            return new TestCaseGeneratorNull();
        }
    }
}

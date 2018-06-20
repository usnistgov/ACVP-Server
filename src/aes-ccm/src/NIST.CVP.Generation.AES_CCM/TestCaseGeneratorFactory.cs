using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAeadModeBlockCipherFactory _cipherFactory;
        private readonly IBlockCipherEngineFactory _engineFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IAeadModeBlockCipherFactory cipherFactory, IBlockCipherEngineFactory engineFactory)
        {
            _random800_90 = random800_90;
            _cipherFactory = cipherFactory;
            _engineFactory = engineFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();
            if (direction == "encrypt")
            {
                return new TestCaseGeneratorEncrypt(_random800_90, _cipherFactory, _engineFactory);
            }

            if (direction == "decrypt")
            {
                return new TestCaseGeneratorDecrypt(_random800_90, _cipherFactory, _engineFactory);
            }

            return new TestCaseGeneratorNull();
        }
    }
}

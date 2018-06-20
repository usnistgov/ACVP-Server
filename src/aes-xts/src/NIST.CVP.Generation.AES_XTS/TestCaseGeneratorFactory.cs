using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory cipherFactory)
        {
            _random800_90 = random800_90;
            _algo = cipherFactory.GetStandardCipher(engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes),
                BlockCipherModesOfOperation.Xts);
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Direction.ToLower();

            if (direction == "encrypt")
            {
                return new TestCaseGeneratorEncrypt(_random800_90, _algo);
            }
            else
            {
                return new TestCaseGeneratorDecrypt(_random800_90, _algo);
            }
        }
    }
}

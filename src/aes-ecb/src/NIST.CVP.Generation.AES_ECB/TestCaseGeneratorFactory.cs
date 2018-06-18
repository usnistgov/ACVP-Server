using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private readonly IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse> _mctAlgo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory cipherFactory, IMonteCarloFactoryAes mctFactory)
        {
            _random800_90 = random800_90;
            _algo = cipherFactory.GetStandardCipher(engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), BlockCipherModesOfOperation.Ecb);
            _mctAlgo = mctFactory.GetInstance(BlockCipherModesOfOperation.Ecb);
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();
            var testType = testGroup.TestType.ToLower();

            switch (testType)
            {
                case "gfsbox":
                case "keysbox":
                case "vartxt":
                case "varkey":
                    return new TestCaseGeneratorKnownAnswer(testGroup.KeyLength, testType);
                case "mct":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMCTEncrypt(_random800_90, _mctAlgo);
                        case "decrypt":
                            return new TestCaseGeneratorMCTDecrypt(_random800_90, _mctAlgo);
                    }
                    break;
                case "mmt":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMMTEncrypt(_random800_90, _algo);
                        case "decrypt":
                            return new TestCaseGeneratorMMTDecrypt(_random800_90, _algo);
                    }
                    break;
            }

            return new TestCaseGeneratorNull();
        }
    }
}

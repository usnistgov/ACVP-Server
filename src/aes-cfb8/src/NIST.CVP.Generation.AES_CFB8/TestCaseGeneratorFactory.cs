using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB8
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IMonteCarloFactoryAes _mctFactory;
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;

        public TestCaseGeneratorFactory(
            IRandom800_90 random800_90,
            IMonteCarloFactoryAes mctFactory,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory
        )
        {
            _random800_90 = random800_90;
            _mctFactory = mctFactory;
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
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
                            return new TestCaseGeneratorMCTEncrypt(testGroup, _random800_90, _mctFactory);
                        case "decrypt":
                            return new TestCaseGeneratorMCTDecrypt(testGroup, _random800_90, _mctFactory);
                    }
                    break;
                case "mmt":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMMTEncrypt(testGroup, _random800_90, _engineFactory, _modeFactory);
                        case "decrypt":
                            return new TestCaseGeneratorMMTDecrypt(testGroup, _random800_90, _engineFactory, _modeFactory);
                    }
                    break;
            }

            return new TestCaseGeneratorNull();
        }
    }
}

using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_OFB
{
    public class TestCaseGeneratorMMTEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;
        private const int NUMBER_OF_CASES = 10;

        private readonly IRandom800_90 _random800_90;
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;

        private int _currentCase;

        public int NumberOfTestCasesToGenerate => NUMBER_OF_CASES;

        public TestCaseGeneratorMMTEncrypt(
            IRandom800_90 random800_90,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory
        )
        {
            _random800_90 = random800_90;
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            //todo separate out keys? isSample?
            var key = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * group.NumberOfKeys).ToOddParityBitString();
            var plainText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * (_currentCase + 1));
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            var testCase = new TestCase
            {
                Key = key,
                PlainText = plainText,
                Iv = iv,
                Deferred = false
            };

            _currentCase++;
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            try
            {
                var algo = _modeFactory.GetStandardCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes),
                    BlockCipherModesOfOperation.Ofb
                );
                var p = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    testCase.Iv,
                    testCase.Key,
                    testCase.PlainText
                );

                var result = algo.ProcessPayload(p);
                if (!result.Success)
                {
                    ThisLogger.Warn(result.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(result.ErrorMessage);
                    }
                }

                testCase.CipherText = result.Result;
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
                }
            }
            
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

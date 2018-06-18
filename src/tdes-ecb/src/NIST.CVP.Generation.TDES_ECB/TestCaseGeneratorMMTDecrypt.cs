using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestCaseGeneratorMMTDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;
        private const int NUMBER_OF_CASES = 10;
        private readonly IRandom800_90 _random800_90;
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private int _currentCase;

        public int NumberOfTestCasesToGenerate => NUMBER_OF_CASES;

        public TestCaseGeneratorMMTDecrypt(IRandom800_90 random800_90, IModeBlockCipher<SymmetricCipherResult> algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var numberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(group.KeyingOption);
            var key = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * numberOfKeys).ToOddParityBitString();
            var cipherText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * (_currentCase + 1));
            var testCase = new TestCase
            {
                Key = key,
                CipherText = cipherText,
                Deferred = false
            };

            _currentCase++;
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            SymmetricCipherResult decryptionResult = null;
            try
            {
                var param = new ModeBlockCipherParameters(BlockCipherDirections.Decrypt, testCase.Key, testCase.CipherText);
                decryptionResult = _algo.ProcessPayload(param);
                if (!decryptionResult.Success)
                {
                    ThisLogger.Warn(decryptionResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(decryptionResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
                }
            }

            testCase.PlainText = decryptionResult.Result;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger(); 
    }
}

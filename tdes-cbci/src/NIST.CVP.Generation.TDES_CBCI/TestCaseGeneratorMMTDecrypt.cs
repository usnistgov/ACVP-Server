using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_CBCI;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;

namespace NIST.CVP.Generation.TDES_CBCI
{
    public class TestCaseGeneratorMMTDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;
        private const int NUMBER_OF_CASES = 10;
        private readonly IRandom800_90 _random800_90;
        private readonly ITDES_CBCI _algo;
        private int _currentCase;

        public TestCaseGeneratorMMTDecrypt(IRandom800_90 random800_90, ITDES_CBCI algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public int NumberOfTestCasesToGenerate => NUMBER_OF_CASES;

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var key = TdesHelpers.GenerateTdesKey(group.KeyingOption); 
            var cipherText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * 3 * (_currentCase + 1));
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            var testCase = new TestCase
            {
                Keys = key,
                CipherText = cipherText,
                IV1 = iv,
                Deferred = false
            };
            _currentCase++;
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            DecryptionResult decryptionResult = null;
            try
            {
                decryptionResult = _algo.BlockDecrypt(testCase.Keys, testCase.IV1, testCase.CipherText);
                if (!decryptionResult.Success)
                {
                    ThisLogger.Warn(decryptionResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse(decryptionResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse(ex.Message);
                }
            }
            testCase.PlainText = decryptionResult.PlainText;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

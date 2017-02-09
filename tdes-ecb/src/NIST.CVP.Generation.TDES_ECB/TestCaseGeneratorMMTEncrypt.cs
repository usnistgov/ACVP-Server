using System;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestCaseGeneratorMMTEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;
        private const int NUMBER_OF_CASES = 10;
        private readonly IRandom800_90 _random800_90;
        private readonly ITDES_ECB _algo;
        private int _currentCase;

        public TestCaseGeneratorMMTEncrypt(IRandom800_90 random800_90, ITDES_ECB algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public int NumberOfTestCasesToGenerate
        {
            get { return NUMBER_OF_CASES; }
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            
            var key = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * @group.NumberOfKeys);
            var plainText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * (_currentCase + 1));
            var testCase = new TestCase
            {
                Key = key,
                PlainText = plainText,
                Deferred = false
            };
            _currentCase++;
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            EncryptionResult encryptionResult = null;
            try
            {
                encryptionResult = _algo.BlockEncrypt(testCase.Key, testCase.PlainText);
                if (!encryptionResult.Success)
                {
                    ThisLogger.Warn(encryptionResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse(encryptionResult.ErrorMessage);
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
            testCase.CipherText = encryptionResult.CipherText;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }

       
    }
}

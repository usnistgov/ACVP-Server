using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_CFB;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using NIST.CVP.Crypto.Common;

namespace NIST.CVP.Generation.TDES_CFB
{
    public class TestCaseGeneratorMMTDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;
        private const int NUMBER_OF_CASES = 10;
        private readonly IRandom800_90 _random800_90;
        private readonly IModeOfOperation _modeOfOperation;
        private int _currentCase;
        private int _shift;

        public TestCaseGeneratorMMTDecrypt(IRandom800_90 random800_90, 
            IModeOfOperation modeOfOperation)
        {
            _random800_90 = random800_90;
            _modeOfOperation = modeOfOperation;
            switch (modeOfOperation.Algo)
            {
                case Algo.TDES_CFB1:
                    _shift = 1;
                    break;
                case Algo.TDES_CFB8:
                    _shift = 8;
                    break;
                case Algo.TDES_CFB64:
                    _shift = 64;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modeOfOperation.Algo), modeOfOperation.Algo, null);
            }
        }

        public int NumberOfTestCasesToGenerate => NUMBER_OF_CASES;

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            var key = TdesHelpers.GenerateTdesKey(group.KeyingOption);
            var cipherText = _random800_90.GetRandomBitString(_shift * (_currentCase + 1));
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            var testCase = new TestCase
            {
                Key = key,
                CipherText = cipherText,
                Iv = iv,
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
                decryptionResult = _modeOfOperation.BlockDecrypt(testCase.Key, testCase.Iv, testCase.CipherText);
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

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }

        }
    }
}
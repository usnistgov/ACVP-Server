using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestCaseGeneratorMMTDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;
        private const int NUMBER_OF_CASES = 10;
        private readonly IRandom800_90 _random800_90;
        private readonly ICFBPMode _modeOfOperation;
        private int _currentCase;
        private int _shift;

        public TestCaseGeneratorMMTDecrypt(IRandom800_90 random800_90, ICFBPMode modeOfOperation)
        {
            _random800_90 = random800_90;
            _modeOfOperation = modeOfOperation;
            switch (modeOfOperation.Algo)
            {
                case AlgoMode.TDES_CFBP1:
                    _shift = 1;
                    break;
                case AlgoMode.TDES_CFBP8:
                    _shift = 8;
                    break;
                case AlgoMode.TDES_CFBP64:
                    _shift = 64;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modeOfOperation.Algo), modeOfOperation.Algo, null);
            }
        }

        public int NumberOfTestCasesToGenerate => NUMBER_OF_CASES;

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            var keys = TdesHelpers.GenerateTdesKey(group.KeyingOption);
            var cipherText = _random800_90.GetRandomBitString(_shift * (_currentCase + 1));
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            var testCase = new TestCase
            {
                Keys = keys,
                CipherText = cipherText,
                IV1 = iv,
                Deferred = false
            };
            _currentCase++;
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            SymmetricCipherWithIvResult decryptionResult = null;
            try
            {
                decryptionResult = _modeOfOperation.BlockDecrypt(testCase.Keys, testCase.IV1, testCase.CipherText);
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
            testCase.PlainText = decryptionResult.Result;
            testCase.IV2 = decryptionResult.IVs[1];
            testCase.IV3 = decryptionResult.IVs[2];
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }

        }
    }
}
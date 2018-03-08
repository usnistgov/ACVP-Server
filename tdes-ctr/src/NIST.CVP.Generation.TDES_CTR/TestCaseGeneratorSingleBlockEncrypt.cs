using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCaseGeneratorSingleBlockEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly ITdesCtr _algo;

        public int NumberOfTestCasesToGenerate { get; } = 10;

        public TestCaseGeneratorSingleBlockEncrypt(IRandom800_90 rand, ITdesCtr algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var plainText = _rand.GetRandomBitString(64);
            var key = _rand.GetRandomBitString(64 * group.NumberOfKeys).ToOddParityBitString();
            var iv = _rand.GetRandomBitString(64);

            var testCase = new TestCase
            {
                PlainText = plainText,
                Key = key,
                Iv = iv,
                Length = plainText.BitLength
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            SymmetricCipherResult encryptionResult = null;
            try
            {
                encryptionResult = _algo.EncryptBlock(testCase.Key, testCase.PlainText, testCase.Iv);
                if (!encryptionResult.Success)
                {
                    ThisLogger.Warn(encryptionResult.ErrorMessage);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(encryptionResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }

            testCase.CipherText = encryptionResult.Result;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

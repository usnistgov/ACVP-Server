using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_CTR;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCaseGeneratorSingleBlockDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly ITdesCtr _algo;

        public int NumberOfTestCasesToGenerate { get; } = 10;

        public TestCaseGeneratorSingleBlockDecrypt(IRandom800_90 rand, ITdesCtr algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var cipherText = _rand.GetRandomBitString(64);
            var key = _rand.GetRandomBitString(64 * group.NumberOfKeys).ToOddParityBitString();
            var iv = _rand.GetRandomBitString(64);

            var testCase = new TestCase
            {
                CipherText = cipherText,
                Key = key,
                Iv = iv,
                Length = cipherText.BitLength
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            DecryptionResult decryptionResult = null;
            try
            {
                decryptionResult = _algo.DecryptBlock(testCase.Key, testCase.CipherText, testCase.Iv);
                if (!decryptionResult.Success)
                {
                    ThisLogger.Warn(decryptionResult.ErrorMessage);
                    return new TestCaseGenerateResponse(decryptionResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse(ex.Message);
            }

            testCase.PlainText = decryptionResult.PlainText;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

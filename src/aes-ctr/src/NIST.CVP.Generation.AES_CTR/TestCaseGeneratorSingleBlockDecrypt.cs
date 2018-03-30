using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseGeneratorSingleBlockDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IAesCtr _algo;

        public int NumberOfTestCasesToGenerate { get; } = 10;

        public TestCaseGeneratorSingleBlockDecrypt(IRandom800_90 rand, IAesCtr algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var cipherText = _rand.GetRandomBitString(128);
            var key = _rand.GetRandomBitString(group.KeyLength);
            var iv = _rand.GetRandomBitString(128);

            var testCase = new TestCase
            {
                CipherText = cipherText,
                Key = key,
                IV = iv,
                Length = 128
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            SymmetricCipherResult decryptionResult = null;
            try
            {
                decryptionResult = _algo.DecryptBlock(testCase.Key, testCase.CipherText, testCase.IV);
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

            testCase.PlainText = decryptionResult.Result;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

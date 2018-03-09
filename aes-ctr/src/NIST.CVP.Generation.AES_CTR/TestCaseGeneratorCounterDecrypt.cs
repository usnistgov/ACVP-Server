using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using Cipher = NIST.CVP.Crypto.Common.Symmetric.CTR.Enums.Cipher;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseGeneratorCounterDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IAesCtr _algo;

        private int _numberOfBlocks = 1000;
        private bool _isSample;

        public int NumberOfTestCasesToGenerate { get; } = 1;

        public TestCaseGeneratorCounterDecrypt(IRandom800_90 rand, IAesCtr algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                _isSample = isSample;
                _numberOfBlocks = 100;
            }

            // Option to potentially include an incomplete block at the end of this
            var cipherText = _rand.GetRandomBitString(128 * _numberOfBlocks);
            var key = _rand.GetRandomBitString(group.KeyLength);
            var iv = GetStartingIV(group);

            var testCase = new TestCase
            {
                CipherText = cipherText,
                Key = key,
                IV = iv,
                Length = cipherText.BitLength,
                Deferred = true
            };

            if (isSample)
            {
                return Generate(group, testCase);
            }
            else
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            SymmetricCounterResult decryptionResult = null;
            try
            {
                // Get a simple counter (has wrapping) starting at the provided IV
                ICounter counter;
                if (group.IncrementalCounter)
                {
                    counter = new AdditiveCounter(Cipher.AES, testCase.IV);
                }
                else
                {
                    counter = new SubtractiveCounter(Cipher.AES, testCase.IV);
                }

                decryptionResult = _algo.Decrypt(testCase.Key, testCase.CipherText, counter);
                if (!decryptionResult.Success)
                {
                    ThisLogger.Warn(decryptionResult.ErrorMessage);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(decryptionResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }

            testCase.PlainText = decryptionResult.Result;
            testCase.IVs = decryptionResult.IVs;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private BitString GetStartingIV(TestGroup group)
        {
            BitString padding;

            // Arbitrary 'small' value so samples and normal registrations always hit boundary
            int randomBits = _isSample ? 6 : 9;

            if (group.OverflowCounter == group.IncrementalCounter)
            {
                padding = BitString.Ones(128 - randomBits);
            }
            else
            {
                padding = BitString.Zeroes(128 - randomBits);
            }

            return BitString.ConcatenateBits(padding, _rand.GetRandomBitString(randomBits));
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

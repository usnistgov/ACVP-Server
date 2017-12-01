using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseGeneratorCounterEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IAesCtr _algo;

        private int _numberOfBlocks = 1000;

        public int NumberOfTestCasesToGenerate { get; } = 1;

        public TestCaseGeneratorCounterEncrypt(IRandom800_90 rand, IAesCtr algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                _numberOfBlocks = 100;
            }

            // Option to potentially include an incomplete block at the end of this
            var plainText = _rand.GetRandomBitString(128 * _numberOfBlocks);
            var key = _rand.GetRandomBitString(128);
            var iv = GetStartingIV(group);

            var testCase = new TestCase
            {
                PlainText = plainText,
                Key = key,
                IV = iv,
                Length = plainText.BitLength,
                Deferred = true
            };

            if (isSample)
            {
                return Generate(group, testCase);
            }
            else
            {
                return new TestCaseGenerateResponse(testCase);
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            CounterEncryptionResult encryptionResult = null;
            try
            {
                // Get a simple counter (has wrapping) starting at the provided IV
                var simpleCounter = new SimpleCounter(testCase.IV);

                encryptionResult = _algo.Encrypt(testCase.Key, testCase.PlainText, simpleCounter);
                if (!encryptionResult.Success)
                {
                    ThisLogger.Warn(encryptionResult.ErrorMessage);
                    return new TestCaseGenerateResponse(encryptionResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse(ex.Message);
            }

            testCase.CipherText = encryptionResult.CipherText;
            testCase.IVs = encryptionResult.IVs;
            return new TestCaseGenerateResponse(testCase);
        }

        private BitString GetStartingIV(TestGroup group)
        {
            BitString padding;
            int randomBits;

            if (group.OverflowCounter == group.IncrementalCounter)
            {
                randomBits = 7;             // Arbitrary 'small' value so samples and normal registrations always hit boundary
                padding = BitString.Ones(128 - randomBits);
            }
            else
            {
                randomBits = 19;            // Arbitrary 'small' value, all counters must support 2^20 messages max per iv, so a 19-bit counter should always work
                padding = BitString.Zeroes(128 - randomBits);
            }

            return BitString.ConcatenateBits(padding, _rand.GetRandomBitString(randomBits));
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}


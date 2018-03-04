using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCaseGeneratorCounterEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly ITdesCtr _algo;

        private int _numberOfBlocks = 1000;
        private bool _isSample;

        public int NumberOfTestCasesToGenerate { get; } = 1;

        public TestCaseGeneratorCounterEncrypt(IRandom800_90 rand, ITdesCtr algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                _isSample = isSample;
                _numberOfBlocks = 100;
            }

            // Option to potentially include an incomplete block at the end of this
            var plainText = _rand.GetRandomBitString(64 * _numberOfBlocks);
            var key = _rand.GetRandomBitString(64 * group.NumberOfKeys).ToOddParityBitString();
            var iv = GetStartingIV(group);

            var testCase = new TestCase
            {
                PlainText = plainText,
                Key = key,
                Iv = iv,
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
            SymmetricCounterResult encryptionResult = null;
            try
            {
                // Get a simple counter (has wrapping) starting at the provided IV
                ICounter counter;
                if (group.IncrementalCounter)
                {
                    counter = new AdditiveCounter(Cipher.TDES, testCase.Iv);
                }
                else
                {
                    counter = new SubtractiveCounter(Cipher.TDES, testCase.Iv);
                }

                encryptionResult = _algo.Encrypt(testCase.Key, testCase.PlainText, counter);
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

            testCase.CipherText = encryptionResult.Result;
            testCase.Ivs = encryptionResult.IVs;
            return new TestCaseGenerateResponse(testCase);
        }

        private BitString GetStartingIV(TestGroup group)
        {
            BitString padding;
            // Arbitrary 'small' value so samples and normal registrations always hit boundary
            int randomBits = _isSample ? 6 : 9;

            if (group.OverflowCounter == group.IncrementalCounter)
            {
                padding = BitString.Ones(64 - randomBits);
            }
            else
            {
                padding = BitString.Zeroes(64 - randomBits);
            }

            return BitString.ConcatenateBits(padding, _rand.GetRandomBitString(randomBits));
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

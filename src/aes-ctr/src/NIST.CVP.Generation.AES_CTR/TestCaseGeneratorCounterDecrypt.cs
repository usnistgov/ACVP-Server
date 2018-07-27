using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseGeneratorCounterDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IBlockCipherEngine _engine;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly ICounterFactory _counterFactory;

        private int _numberOfBlocks = 1000;
        private bool _isSample;

        public int NumberOfTestCasesToGenerate { get; } = 1;

        public TestCaseGeneratorCounterDecrypt(IRandom800_90 rand, IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, ICounterFactory counterFactory)
        {
            _rand = rand;
            _engine = engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes);
            _modeFactory = modeFactory;
            _counterFactory = counterFactory;
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
            try
            {
                // Get a simple counter (has wrapping) starting at the provided IV
                var counterType = group.IncrementalCounter ?
                    CounterTypes.Additive : CounterTypes.Subtractive;
                var counter = _counterFactory.GetCounter(_engine, counterType, testCase.IV);

                var algo = _modeFactory.GetCounterCipher(_engine, counter);
                var result = algo.ProcessPayload(new ModeBlockCipherParameters(
                    BlockCipherDirections.Decrypt,
                    testCase.Key.GetDeepCopy(),
                    testCase.CipherText.GetDeepCopy()
                ));

                if (!result.Success)
                {
                    ThisLogger.Warn(result.ErrorMessage);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(result.ErrorMessage);
                }

                testCase.PlainText = result.Result;
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }

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

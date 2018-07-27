using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCaseGeneratorSingleBlockDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IBlockCipherEngine _engine;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly ICounterFactory _counterFactory;

        public int NumberOfTestCasesToGenerate { get; } = 10;

        public TestCaseGeneratorSingleBlockDecrypt(
            IRandom800_90 random800_90,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory,
            ICounterFactory counterFactory
        )
        {
            _rand = random800_90;
            _engine = engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes);
            _modeFactory = modeFactory;
            _counterFactory = counterFactory;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
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

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            try
            {
                var algo = _modeFactory.GetCounterCipher(
                    _engine,
                    _counterFactory.GetCounter(
                        _engine,
                        CounterTypes.Additive,
                        testCase.Iv.GetDeepCopy()
                    ));
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

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

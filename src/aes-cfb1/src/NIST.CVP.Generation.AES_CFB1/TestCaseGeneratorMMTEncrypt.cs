using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class TestCaseGeneratorMMTEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;

        private int _ptLenGenIteration = 1;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGeneratorMMTEncrypt(
            TestGroup group,
            IRandom800_90 random800_90,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory
        )
        {
            _random800_90 = random800_90;
            var mapping = AlgoModeToEngineModeOfOperationMapping.GetMapping(group.AlgoMode);
            var engine = engineFactory.GetSymmetricCipherPrimitive(mapping.engine);
            _algo = modeFactory.GetStandardCipher(engine, mapping.mode);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            var key = _random800_90.GetRandomBitString(group.KeyLength);
            var plainText = _random800_90.GetRandomBitString(_ptLenGenIteration).GetMostSignificantBits(_ptLenGenIteration++);
            var iv = _random800_90.GetRandomBitString(128);
            var testCase = new TestCase
            {
                DataLen = _ptLenGenIteration,
                IV = iv,
                Key = key,
                PlainText = plainText
            };
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase testCase)
        {
            try
            {
                var result = _algo.ProcessPayload(new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    testCase.IV.GetDeepCopy(),
                    testCase.Key.GetDeepCopy(),
                    testCase.PlainText.GetDeepCopy()
                ));
                if (!result.Success)
                {
                    ThisLogger.Warn(result.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(result.ErrorMessage);
                    }
                }

                testCase.CipherText = result.Result;
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
                }
            }
            
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

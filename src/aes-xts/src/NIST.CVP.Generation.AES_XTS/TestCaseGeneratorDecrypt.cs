using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestCaseGeneratorDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;

        public int NumberOfTestCasesToGenerate { get; } = 50;

        public TestCaseGeneratorDecrypt(IRandom800_90 rand, IModeBlockCipher<SymmetricCipherResult> algo)
        {
            _random800_90 = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var key = _random800_90.GetRandomBitString(group.KeyLen * 2);
            var cipherText = _random800_90.GetRandomBitString(group.PtLen);

            var i = new BitString(0);
            var number = 0;

            if (group.TweakMode.Equals("hex", StringComparison.OrdinalIgnoreCase))
            {
                i = _random800_90.GetRandomBitString(128);
            }
            else if (group.TweakMode.Equals("number", StringComparison.OrdinalIgnoreCase))
            {
                number = _random800_90.GetRandomInt(0, 256);
                i = XtsHelper.GetIFromInteger(number);
            }

            var testCase = new TestCase
            {
                CipherText = cipherText,
                XtsKey = new XtsKey(key),
                I = i,
                SequenceNumber = number
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            SymmetricCipherResult decryptionResult = null;
            try
            {
                var param = new ModeBlockCipherParameters(BlockCipherDirections.Decrypt, testCase.I, testCase.Key,
                    testCase.CipherText);

                decryptionResult = _algo.ProcessPayload(param);
                if (!decryptionResult.Success)
                {
                    ThisLogger.Warn(decryptionResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(decryptionResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
                }
            }

            testCase.PlainText = decryptionResult.Result;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

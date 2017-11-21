using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_XTS;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestCaseGeneratorDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAesXts _algo;

        public int NumberOfTestCasesToGenerate { get; } = 50;

        public TestCaseGeneratorDecrypt(IRandom800_90 rand, IAesXts algo)
        {
            _random800_90 = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
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
                i = _algo.GetIFromInteger(number);
            }

            var testCase = new TestCase
            {
                CipherText = cipherText,
                Key = new XtsKey(key),
                I = i,
                SequenceNumber = number
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            DecryptionResult decryptionResult = null;
            try
            {
                decryptionResult = _algo.Decrypt(testCase.Key, testCase.CipherText, testCase.I);
                if (!decryptionResult.Success)
                {
                    ThisLogger.Warn(decryptionResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse(decryptionResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse(ex.Message);
                }
            }

            testCase.PlainText = decryptionResult.PlainText;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}

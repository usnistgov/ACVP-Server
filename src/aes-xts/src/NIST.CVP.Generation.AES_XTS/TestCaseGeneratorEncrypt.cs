using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_XTS;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestCaseGeneratorEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAesXts _algo;

        public int NumberOfTestCasesToGenerate { get; private set; } = 50;

        public TestCaseGeneratorEncrypt(IRandom800_90 rand, IAesXts algo)
        {
            _random800_90 = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 10;
            }

            var key = _random800_90.GetRandomBitString(group.KeyLen * 2);
            var plainText = _random800_90.GetRandomBitString(group.PtLen);

            var i = new BitString(0);
            var number = 0;

            if (group.TweakMode.Equals("hex", StringComparison.OrdinalIgnoreCase))
            {
                i = _random800_90.GetRandomBitString(128);
            }
            else // if (group.TweakMode.Equals("number", StringComparison.OrdinalIgnoreCase))
            {
                number = _random800_90.GetRandomInt(0, 256);
                i = _algo.GetIFromInteger(number);
            }

            var testCase = new TestCase
            {
                PlainText = plainText,
                Key = new XtsKey(key),
                I = i,
                SequenceNumber = number
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            SymmetricCipherResult encryptionResult = null;
            try
            {
                encryptionResult = _algo.Encrypt(testCase.Key, testCase.PlainText, testCase.I);
                if (!encryptionResult.Success)
                {
                    ThisLogger.Warn(encryptionResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse(encryptionResult.ErrorMessage);
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

            testCase.CipherText = encryptionResult.Result;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}


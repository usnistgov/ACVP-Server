using System;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class TestCaseGeneratorMMTEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_CFB1 _algo;

        private int _ptLenGenIteration = 1;

        public int NumberOfTestCasesToGenerate { get { return 10; } }

        public TestCaseGeneratorMMTEncrypt(IRandom800_90 random800_90, IAES_CFB1 algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            var key = _random800_90.GetRandomBitString(group.KeyLength);
            var plainText = _random800_90.GetRandomBitString(_ptLenGenIteration).GetMostSignificantBits(_ptLenGenIteration++);
            var iv = _random800_90.GetRandomBitString(Cipher._MAX_IV_BYTE_LENGTH * 8);
            var testCase = new TestCase
            {
                IV = iv,
                Key = key,
                PlainText = BitOrientedBitString.GetDerivedFromBase(plainText),
                Deferred = false
            };
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            EncryptionResult encryptionResult = null;
            try
            {
                encryptionResult = _algo.BlockEncrypt(testCase.IV, testCase.Key, testCase.PlainText);
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
            testCase.CipherText = BitOrientedBitString.GetDerivedFromBase(encryptionResult.CipherText);
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}

using System;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CBC
{
    public class TestCaseGeneratorEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_CBC _algo;

        public TestCaseGeneratorEncrypt(IRandom800_90 random800_90, IAES_CBC algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            
            var key = _random800_90.GetRandomBitString(group.KeyLength);
            var plainText = _random800_90.GetRandomBitString(group.PTLength);
            var iv = _random800_90.GetRandomBitString(Cipher._MAX_IV_BYTE_LENGTH * 8);
            var testCase = new TestCase
            {
                IV = iv,
                Key = key,
                PlainText = plainText,
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
            testCase.CipherText = encryptionResult.CipherText;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}

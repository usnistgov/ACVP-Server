using System;
using NIST.CVP.Crypto.AES_GCM;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseGeneratorInternalEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_GCM _aes_gcm;

        public int NumberOfTestCasesToGenerate { get { return 15; } }

        public TestCaseGeneratorInternalEncrypt(IRandom800_90 random800_90, IAES_GCM aes_gcm)
        {
            _random800_90 = random800_90;
            _aes_gcm = aes_gcm;
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            //no known answer, but we need the prompts
            var key = _random800_90.GetRandomBitString(group.KeyLength);
            var plainText = _random800_90.GetRandomBitString(group.PTLength);
            var aad = _random800_90.GetRandomBitString(group.AADLength);

            var testCase = new TestCase
            {
                Key = key,
                AAD = aad,
                PlainText = plainText,
                Deferred = true
            };

            // if a sample is requested, we need to generate an IV and go through with the actual encryption like we do for External
            if (isSample)
            {
                testCase.IV = _random800_90.GetRandomBitString(group.IVLength);
                return Generate(group, testCase);
            }
            
            return new TestCaseGenerateResponse(testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            EncryptionResult encryptionResult = null;
            try
            {
                encryptionResult = _aes_gcm.BlockEncrypt(testCase.Key, testCase.PlainText, testCase.IV, testCase.AAD, @group.TagLength);
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
            testCase.Tag = encryptionResult.Tag;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}

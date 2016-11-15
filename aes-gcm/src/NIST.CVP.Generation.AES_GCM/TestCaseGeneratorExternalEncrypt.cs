using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseGeneratorExternalEncrypt : ITestCaseGenerator
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_GCM _aes_gcm;

        public TestCaseGeneratorExternalEncrypt(IRandom800_90 random800_90, IAES_GCM aes_gcm)
        {
            _random800_90 = random800_90;
            _aes_gcm = aes_gcm;
        }

        public string IVGen { get { return "external"; } }
        public string Direction { get { return "encrypt"; } }

        public TestCaseGenerateResponse Generate(TestGroup @group)
        {
            //known answer
            var iv = _random800_90.GetRandomBitString(@group.IVLength);
            var key = _random800_90.GetRandomBitString(group.KeyLength);
            var plainText = _random800_90.GetRandomBitString(group.PTLength);
            var aad = _random800_90.GetRandomBitString(group.AADLength);
            var testCase = new TestCase
            {
                IV = iv,
                AAD = aad,
                Key = key,
                PlainText = plainText,
                Deferred = false
            };
            return Generate(@group, testCase);

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

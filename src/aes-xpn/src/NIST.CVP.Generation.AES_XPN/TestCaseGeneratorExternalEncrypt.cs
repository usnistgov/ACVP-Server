using System;
using NIST.CVP.Crypto.AES_GCM;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_XPN
{
    public class TestCaseGeneratorExternalEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_GCM _aes_gcm;

        public int NumberOfTestCasesToGenerate { get { return 15; } }

        public TestCaseGeneratorExternalEncrypt(IRandom800_90 random800_90, IAES_GCM aes_gcm)
        {
            _random800_90 = random800_90;
            _aes_gcm = aes_gcm;
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            //known answer - need to do an encryption operation to get the tag
            var key = _random800_90.GetRandomBitString(@group.KeyLength);
            var iv = _random800_90.GetRandomBitString(@group.IVLength);
            var salt = _random800_90.GetRandomBitString(@group.SaltLength);
            var plainText = _random800_90.GetRandomBitString(group.PTLength);
            var aad = _random800_90.GetRandomBitString(group.AADLength);
            var testCase = new TestCase
            {
                Key = key,
                IV = iv,
                Salt = salt,
                AAD = aad,
                PlainText = plainText,
                Deferred = false
            };
            return Generate(@group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            SymmetricCipherAeadResult encryptionResult = null;
            try
            {
                var ivXorSalt = testCase.Salt.XOR(testCase.IV);

                encryptionResult = _aes_gcm.BlockEncrypt(testCase.Key, testCase.PlainText, ivXorSalt, testCase.AAD, @group.TagLength);
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

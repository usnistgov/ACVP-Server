using System;
using NIST.CVP.Crypto.AES_GCM;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseGeneratorDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IAES_GCM _aesGcm;
        private readonly IRandom800_90 _random800_90;

        public int NumberOfTestCasesToGenerate { get { return 15; } }

        public TestCaseGeneratorDecrypt(IRandom800_90 random800_90, IAES_GCM aesGcm)
        {
            _random800_90 = random800_90;
            _aesGcm = aesGcm;
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            //known answer - need to do an encryption operation to get the tag
            var key = _random800_90.GetRandomBitString(@group.KeyLength);
            var iv = _random800_90.GetRandomBitString(@group.IVLength);
            var plainText = _random800_90.GetRandomBitString(group.PTLength);
            var aad = _random800_90.GetRandomBitString(group.AADLength);
            var testCase = new TestCase
            {
                Key = key,
                IV = iv,
                AAD = aad,
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
                encryptionResult = _aesGcm.BlockEncrypt(testCase.Key, testCase.PlainText, testCase.IV, testCase.AAD, @group.TagLength);
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

            SometimesMangleTestCaseTag(testCase);

            return new TestCaseGenerateResponse(testCase);
        }

        private void SometimesMangleTestCaseTag(TestCase testCase)
        {
            // Alter the tag 25% of the time for a "failure" test
            int option = _random800_90.GetRandomInt(0, 4);
            if (option == 0)
            {
                testCase.Tag = _random800_90.GetDifferentBitStringOfSameSize(testCase.Tag);
                testCase.FailureTest = true;
            }
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
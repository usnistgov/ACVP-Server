using System;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestCaseGeneratorDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_CCM _algo;
        
        public int NumberOfTestCasesToGenerate { get { return 15; } }

        public TestCaseGeneratorDecrypt(IRandom800_90 random800_90, IAES_CCM algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
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
                encryptionResult = _algo.Encrypt(testCase.Key, testCase.IV, testCase.PlainText, testCase.AAD, @group.TagLength);
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

            SometimesMangleTestCaseCipherText(testCase);

            return new TestCaseGenerateResponse(testCase);
        }

        private void SometimesMangleTestCaseCipherText(TestCase testCase)
        {
            // Alter the ciphertext 25% of the time for a "failure" test
            int option = _random800_90.GetRandomInt(0, 4);
            if (option == 0)
            {
                testCase.CipherText = _random800_90.GetDifferentBitStringOfSameSize(testCase.CipherText);
                testCase.FailureTest = true;
            }
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
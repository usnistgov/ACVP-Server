using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class TestCaseGeneratorMMTDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IAES_CFB1 _algo;
        private readonly IRandom800_90 _random800_90;
        
        private int _ctLenGenIteration = 1;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGeneratorMMTDecrypt(IRandom800_90 random800_90, IAES_CFB1 algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            //known answer - need to do an encryption operation to get the tag
            var key = _random800_90.GetRandomBitString(@group.KeyLength);
            var cipherText = _random800_90.GetRandomBitString(_ctLenGenIteration).GetMostSignificantBits(_ctLenGenIteration++);
            var iv = _random800_90.GetRandomBitString((Cipher._MAX_IV_BYTE_LENGTH * 8));
            var testCase = new TestCase
            {
                DataLen = _ctLenGenIteration,
                IV = iv,
                Key = key,
                CipherText = cipherText
            };
            return Generate(@group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase testCase)
        {
            SymmetricCipherResult decryptionResult = null;
            try
            {
                decryptionResult = _algo.BlockDecrypt(testCase.IV.GetDeepCopy(), testCase.Key, testCase.CipherText);
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
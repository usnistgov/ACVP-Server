using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CFB1;
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

        public int NumberOfTestCasesToGenerate { get { return 10; } }

        public TestCaseGeneratorMMTDecrypt(IRandom800_90 random800_90, IAES_CFB1 algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            //known answer - need to do an encryption operation to get the tag
            var key = _random800_90.GetRandomBitString(@group.KeyLength);
            var cipherText = _random800_90.GetRandomBitString(_ctLenGenIteration).GetMostSignificantBits(_ctLenGenIteration++);
            var iv = _random800_90.GetRandomBitString((Cipher._MAX_IV_BYTE_LENGTH * 8));
            var testCase = new TestCase
            {
                IV = iv,
                Key = key,
                CipherText = BitOrientedBitString.GetDerivedFromBase(cipherText),
                Deferred = false
            };
            return Generate(@group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            DecryptionResult decryptionResult = null;
            try
            {
                decryptionResult = _algo.BlockDecrypt(testCase.IV.GetDeepCopy(), testCase.Key, testCase.CipherText);
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

            testCase.PlainText = BitOrientedBitString.GetDerivedFromBase(decryptionResult.PlainText);
            return new TestCaseGenerateResponse(testCase);
        }

      
        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
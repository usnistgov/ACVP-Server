using System;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestCaseGeneratorDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly ITDES_ECB _algo;
        private readonly IRandom800_90 _random800_90;


        public TestCaseGeneratorDecrypt(IRandom800_90 random800_90, ITDES_ECB algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }
        
        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            //known answer - need to do an encryption operation to get the tag
            var key = _random800_90.GetRandomBitString(@group.KeyLength);
            var plainText = _random800_90.GetRandomBitString(group.PTLength);
            var testCase = new TestCase
            {
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
                encryptionResult = _algo.BlockEncrypt(testCase.Key, testCase.PlainText);
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

        public int NumberOfTestCasesToGenerate { get; set; }
    }
}
using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_OFB
{
    public class TestCaseGeneratorMCTDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _iRandom80090;
        private readonly IAES_OFB_MCT _iAesOfbMct;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMCTDecrypt(IRandom800_90 iRandom80090, IAES_OFB_MCT iAesOfbMct)
        {
            _iRandom80090 = iRandom80090;
            _iAesOfbMct = iAesOfbMct;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            var iv = _iRandom80090.GetRandomBitString(128);
            var key = _iRandom80090.GetRandomBitString(@group.KeyLength);
            var cipherText = _iRandom80090.GetRandomBitString(128);
            TestCase testCase = new TestCase()
            {
                IV = iv,
                Key = key,
                CipherText = cipherText,
                Deferred = false
            };

            return Generate(@group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase testCase)
        {
            MCTResult<AlgoArrayResponse> decryptionResult = null;
            try
            {
                decryptionResult = _iAesOfbMct.MCTDecrypt(testCase.IV, testCase.Key, testCase.CipherText);
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
            testCase.ResultsArray = decryptionResult.Response;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

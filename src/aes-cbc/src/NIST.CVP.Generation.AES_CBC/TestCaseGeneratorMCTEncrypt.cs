using NIST.CVP.Generation.Core;
using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CBC
{
    public class TestCaseGeneratorMCTEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _iRandom80090;
        private readonly IAES_CBC_MCT _iAesEcbMct;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMCTEncrypt(IRandom800_90 iRandom80090, IAES_CBC_MCT iAesEcbMct)
        {
            _iRandom80090 = iRandom80090;
            _iAesEcbMct = iAesEcbMct;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            var iv = _iRandom80090.GetRandomBitString(128);
            var key = _iRandom80090.GetRandomBitString(@group.KeyLength);
            var plainText = _iRandom80090.GetRandomBitString(128);
            TestCase testCase = new TestCase()
            {
                IV = iv,
                Key = key,
                PlainText = plainText,
                Deferred = false
            };

            return Generate(@group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase testCase)
        {
            MCTResult<AlgoArrayResponse> encryptionResult = null;
            try
            {
                encryptionResult = _iAesEcbMct.MCTEncrypt(testCase.IV, testCase.Key, testCase.PlainText);
                if (!encryptionResult.Success)
                {
                    ThisLogger.Warn(encryptionResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(encryptionResult.ErrorMessage);
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
            testCase.ResultsArray = encryptionResult.Response;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

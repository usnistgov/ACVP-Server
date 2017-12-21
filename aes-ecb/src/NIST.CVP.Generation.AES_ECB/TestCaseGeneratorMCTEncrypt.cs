using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestCaseGeneratorMCTEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _iRandom80090;
        private readonly IAES_ECB_MCT _iAesEcbMct;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMCTEncrypt(IRandom800_90 iRandom80090, IAES_ECB_MCT iAesEcbMct)
        {
            _iRandom80090 = iRandom80090;
            _iAesEcbMct = iAesEcbMct;
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            var key = _iRandom80090.GetRandomBitString(@group.KeyLength);
            var plainText = _iRandom80090.GetRandomBitString(128);
            TestCase testCase = new TestCase()
            {
                Key = key,
                PlainText = plainText,
                Deferred = false
            };

            return Generate(@group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            MCTResult<AlgoArrayResponse> encryptionResult = null;
            try
            {
                encryptionResult = _iAesEcbMct.MCTEncrypt(testCase.Key, testCase.PlainText);
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
            testCase.ResultsArray = encryptionResult.Response;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}

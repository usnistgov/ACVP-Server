using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CFB128
{
    public class TestCaseGeneratorMCTEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _iRandom80090;
        private readonly IAES_CFB128_MCT _algo;

        public int NumberOfTestCasesToGenerate { get { return 1; } }

        public TestCaseGeneratorMCTEncrypt(IRandom800_90 iRandom80090, IAES_CFB128_MCT iAlgo)
        {
            _iRandom80090 = iRandom80090;
            _algo = iAlgo;
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
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

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            MCTResult<AlgoArrayResponse> encryptionResult = null;
            try
            {
                encryptionResult = _algo.MCTEncrypt(testCase.IV, testCase.Key, testCase.PlainText);
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

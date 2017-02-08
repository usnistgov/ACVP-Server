using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class TestCaseGeneratorMCTEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _iRandom80090;
        private readonly IAES_CFB1_MCT _iAesOfbMct;

        public int NumberOfTestCasesToGenerate { get { return 1; } }

        public TestCaseGeneratorMCTEncrypt(IRandom800_90 iRandom80090, IAES_CFB1_MCT iAesOfbMct)
        {
            _iRandom80090 = iRandom80090;
            _iAesOfbMct = iAesOfbMct;
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            var iv = _iRandom80090.GetRandomBitString(128);
            var key = _iRandom80090.GetRandomBitString(@group.KeyLength);
            var plainText = _iRandom80090.GetRandomBitString(1).GetMostSignificantBits(1);
            TestCase testCase = new TestCase()
            {
                IV = iv,
                Key = key,
                PlainText = BitOrientedBitString.GetDerivedFromBase(plainText),
                Deferred = false
            };

            return Generate(@group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            MCTResult<BitOrientedAlgoArrayResponse> encryptionResult = null;
            try
            {
                encryptionResult = _iAesOfbMct.MCTEncrypt(testCase.IV, testCase.Key, testCase.PlainText);
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

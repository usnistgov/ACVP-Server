using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CBC
{
    public class TestCaseGeneratorMCTDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _iRandom80090;
        private readonly IMonteCarloFactoryAes _mctFactory;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMCTDecrypt(IRandom800_90 iRandom80090, IMonteCarloFactoryAes mctFactory)
        {
            _iRandom80090 = iRandom80090;
            _mctFactory = mctFactory;
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
            MCTResult<AlgoArrayResponse> result = null;
            try
            {
                var mct = _mctFactory.GetInstance(BlockCipherModesOfOperation.Cbc);
                var p = new ModeBlockCipherParameters(
                    BlockCipherDirections.Decrypt,
                    testCase.IV.GetDeepCopy(),
                    testCase.Key.GetDeepCopy(),
                    testCase.CipherText.GetDeepCopy()
                );

                result = mct.ProcessMonteCarloTest(p);
                if (!result.Success)
                {
                    ThisLogger.Warn(result.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(result.ErrorMessage);
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
            testCase.ResultsArray = result.Response;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

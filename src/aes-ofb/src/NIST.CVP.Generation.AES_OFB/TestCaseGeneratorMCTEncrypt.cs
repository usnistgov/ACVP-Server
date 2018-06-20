using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_OFB
{
    public class TestCaseGeneratorMCTEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _iRandom80090;
        private readonly IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse> _mctAlgo;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMCTEncrypt(IRandom800_90 iRandom80090, IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse> mctAlgo)
        {
            _iRandom80090 = iRandom80090;
            _mctAlgo = mctAlgo;
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
                var param = new ModeBlockCipherParameters(BlockCipherDirections.Encrypt, testCase.IV, testCase.Key, testCase.PlainText);
                encryptionResult = _mctAlgo.ProcessMonteCarloTest(param);
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

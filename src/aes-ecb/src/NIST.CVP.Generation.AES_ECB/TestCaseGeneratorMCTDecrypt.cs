﻿using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestCaseGeneratorMCTDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _iRandom80090;
        private readonly IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse> _algo;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMCTDecrypt(IRandom800_90 iRandom80090, IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse> algo)
        {
            _iRandom80090 = iRandom80090;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            var key = _iRandom80090.GetRandomBitString(@group.KeyLength);
            var cipherText = _iRandom80090.GetRandomBitString(128);
            TestCase testCase = new TestCase()
            {
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
                var param = new ModeBlockCipherParameters(BlockCipherDirections.Decrypt, testCase.Key, testCase.CipherText);
                decryptionResult = _algo.ProcessMonteCarloTest(param);
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
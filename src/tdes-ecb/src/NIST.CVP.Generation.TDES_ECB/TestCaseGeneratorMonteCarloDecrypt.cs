﻿using System;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestCaseGeneratorMonteCarloDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;
        private readonly IRandom800_90 _random800_90;
        private readonly IMonteCarloTester<Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse>, AlgoArrayResponse> _algo;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMonteCarloDecrypt(IRandom800_90 random800_90, IMonteCarloTester<Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse>, AlgoArrayResponse> algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var seedCase = GetSeedCase(group);

            return Generate(@group, seedCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase seedCase)
        {
            Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse> decryptionResult = null;
            try
            {
                var param = new ModeBlockCipherParameters(BlockCipherDirections.Decrypt, seedCase.Key, seedCase.CipherText);
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

            seedCase.ResultsArray = decryptionResult.Response;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(seedCase);
        }

        private TestCase GetSeedCase(TestGroup group)
        {
            var numberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(group.KeyingOption);
            var key = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * numberOfKeys).ToOddParityBitString();
            var cipherText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase { Key = key, CipherText = cipherText };
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
﻿using System;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_OFB
{
    public class TestCaseGeneratorMonteCarloDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly IMonteCarloFactoryTdes _mctFactory;

        public int NumberOfTestCasesToGenerate { get; private set; } = 1;

        public TestCaseGeneratorMonteCarloDecrypt(
            IRandom800_90 random800_90, 
            IMonteCarloFactoryTdes mctFactory
        )
        {
            _random800_90 = random800_90;
            _mctFactory = mctFactory;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var seedCase = GetSeedCase(group);

            return Generate(group, seedCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase seedCase)
        {
            try
            {
                var mct = _mctFactory.GetInstance(BlockCipherModesOfOperation.Ofb);
                var p = new ModeBlockCipherParameters(
                    BlockCipherDirections.Decrypt,
                    seedCase.Iv.GetDeepCopy(),
                    seedCase.Key.GetDeepCopy(),
                    seedCase.CipherText.GetDeepCopy()
                );

                var result = mct.ProcessMonteCarloTest(p);
                if (!result.Success)
                {
                    ThisLogger.Warn(result.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(result.ErrorMessage);
                    }
                }

                seedCase.ResultsArray = result.Response;
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
                }
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(seedCase);
        }

        private TestCase GetSeedCase(TestGroup group)
        {
            var key = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * group.NumberOfKeys).ToOddParityBitString();
            var cipherText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase { Key = key, CipherText = cipherText, Iv = iv };
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_CBCI
{
    public class TestCaseGeneratorMonteCarloDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly IMonteCarloFactoryTdesPartitions _mctFactory;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMonteCarloDecrypt(
            IRandom800_90 iRandom80090,
            IMonteCarloFactoryTdesPartitions mctFactory
        )
        {
            _random800_90 = iRandom80090;
            _mctFactory = mctFactory;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            var seedCase = GetSeedCase(group);

            return Generate(group, seedCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase seedCase)
        {
            try
            {
                var mct = _mctFactory.GetInstance(BlockCipherModesOfOperation.Cbci);
                var p = new ModeBlockCipherParameters(
                    BlockCipherDirections.Decrypt,
                    seedCase.IV1.GetDeepCopy(),
                    seedCase.Keys.GetDeepCopy(),
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
            var key = TdesHelpers.GenerateTdesKey(group.KeyingOption);
            var cipherText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * 3);
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase { Keys = key, CipherText = cipherText, IV1 = iv };
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_OFBI
{
    public class TestCaseGeneratorMonteCarloEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly IMonteCarloFactoryTdesPartitions _mctFactory;

        public TestCaseGeneratorMonteCarloEncrypt(
            IRandom800_90 random800_90,
            IMonteCarloFactoryTdesPartitions mctFactory
        )
        {
            _random800_90 = random800_90;
            _mctFactory = mctFactory;
        }

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var seedCase = GetSeedCase(group);

            return Generate(group, seedCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase seedCase)
        {
            try
            {
                var mct = _mctFactory.GetInstance(BlockCipherModesOfOperation.Ofbi);
                var p = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    seedCase.IV1.GetDeepCopy(),
                    seedCase.Keys.GetDeepCopy(),
                    seedCase.PlainText.GetDeepCopy()
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
            var plainText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * 3);
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase { Keys = key, PlainText = plainText, IV1 = iv };
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

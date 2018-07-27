using System;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using AlgoArrayResponse = NIST.CVP.Crypto.Common.Symmetric.TDES.AlgoArrayResponse;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class TestCaseGeneratorMonteCarloEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly IMonteCarloFactoryTdes _mctFactory;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMonteCarloEncrypt(
            IRandom800_90 iRandom80090, 
            IMonteCarloFactoryTdes mctFactory
        )
        {
            _random800_90 = iRandom80090;
            _mctFactory = mctFactory;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            var seedCase = GetSeedCase(@group);

            return Generate(@group, seedCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase seedCase)
        {
            Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse> result = null;
            try
            {
                var mct = _mctFactory.GetInstance(BlockCipherModesOfOperation.Cbc);
                var p = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    seedCase.Iv.GetDeepCopy(),
                    seedCase.Key.GetDeepCopy(),
                    seedCase.PlainText.GetDeepCopy()
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
            seedCase.ResultsArray = result.Response;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(seedCase);
        }

        private TestCase GetSeedCase(TestGroup @group)
        {
            var numberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(group.KeyingOption);
            var key = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * numberOfKeys).ToOddParityBitString();
            var plainText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase { Key = key, PlainText = plainText, Iv = iv};
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

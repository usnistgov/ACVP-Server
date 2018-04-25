using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_OFBI
{
    public class TestCaseGeneratorMonteCarloEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly ITDES_OFBI_MCT _algo;

        public TestCaseGeneratorMonteCarloEncrypt(IRandom800_90 random800_90, ITDES_OFBI_MCT algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var seedCase = GetSeedCase(group);

            return Generate(group, seedCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase seedCase)
        {
            MCTResult<AlgoArrayResponseWithIvs> encryptionResult = null;
            try
            {
                encryptionResult = _algo.MCTEncrypt(seedCase.Keys, seedCase.IV1, seedCase.PlainText);
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
            seedCase.ResultsArray = encryptionResult.Response;
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

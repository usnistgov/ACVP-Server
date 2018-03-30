using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_OFBI;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_OFBI
{
    public class TestCaseGeneratorMonteCarloDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly ITDES_OFBI_MCT _algo;

        public TestCaseGeneratorMonteCarloDecrypt(IRandom800_90 random800_90, ITDES_OFBI_MCT algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            var seedCase = GetSeedCase(group);

            return Generate(group, seedCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase seedCase)
        {
            MCTResult<AlgoArrayResponseWithIvs> decryptionResult = null;
            try
            {
                decryptionResult = _algo.MCTDecrypt(seedCase.Keys, seedCase.IV1, seedCase.CipherText);
                if (!decryptionResult.Success)
                {
                    ThisLogger.Warn(decryptionResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse(decryptionResult.ErrorMessage);
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
            seedCase.ResultsArray = decryptionResult.Response;
            return new TestCaseGenerateResponse(seedCase);
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

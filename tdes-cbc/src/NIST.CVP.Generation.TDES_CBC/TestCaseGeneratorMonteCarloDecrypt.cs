using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_CBC;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class TestCaseGeneratorMonteCarloDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly ITDES_CBC_MCT _algo;

        public TestCaseGeneratorMonteCarloDecrypt(IRandom800_90 random800_90, ITDES_CBC_MCT algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public int NumberOfTestCasesToGenerate
        {
            get { return 1; }
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            var seedCase = GetSeedCase(@group);

            return Generate(@group, seedCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase seedCase)
        {
            MCTResult<AlgoArrayResponse> decryptionResult = null;
            try
            {
                decryptionResult = _algo.MCTDecrypt(seedCase.Key, seedCase.CipherText, seedCase.Iv);
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

        private TestCase GetSeedCase(TestGroup @group)
        {
            var numberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(group.KeyingOption);
            var key = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * numberOfKeys).ToOddParityBitString();
            var cipherText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase { Key = key, CipherText = cipherText, Iv = iv};
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }


    }
}

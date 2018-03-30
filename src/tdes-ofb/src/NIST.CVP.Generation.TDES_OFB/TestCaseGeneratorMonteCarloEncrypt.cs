using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_OFB;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_OFB
{
    public class TestCaseGeneratorMonteCarloEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly ITDES_OFB_MCT _algo;

        public TestCaseGeneratorMonteCarloEncrypt(IRandom800_90 random800_90, ITDES_OFB_MCT algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public int NumberOfTestCasesToGenerate
        {
            get
            {
                return 1;
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            var seedCase = GetSeedCase(@group);

            return Generate(@group, seedCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase seedCase)
        {
            MCTResult<AlgoArrayResponse> encryptionResult = null;
            try
            {
                encryptionResult = _algo.MCTEncrypt(seedCase.Key, seedCase.PlainText, seedCase.Iv);
                if (!encryptionResult.Success)
                {
                    ThisLogger.Warn(encryptionResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse(encryptionResult.ErrorMessage);
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
            seedCase.ResultsArray = encryptionResult.Response;
            return new TestCaseGenerateResponse(seedCase);
        }

        private TestCase GetSeedCase(TestGroup @group)
        {
            var key = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * @group.NumberOfKeys).ToOddParityBitString();
            var plainText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase { Key = key, PlainText = plainText, Iv = iv };
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }


    }
}

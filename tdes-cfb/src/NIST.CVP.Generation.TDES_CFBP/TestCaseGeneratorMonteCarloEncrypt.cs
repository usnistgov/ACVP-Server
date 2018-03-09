using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestCaseGeneratorMonteCarloEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly int _shift;
        private readonly ICFBPModeMCT _mode;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMonteCarloEncrypt(IRandom800_90 random800_90, ICFBPModeMCT mode)
        {
            switch (mode.Algo)
            {
                case AlgoMode.TDES_CFBP1:
                    _shift = 1;
                    break;
                case AlgoMode.TDES_CFBP8:
                    _shift = 8;
                    break;
                case AlgoMode.TDES_CFBP64:
                    _shift = 64;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode.Algo), mode.Algo, null);
            }
            _random800_90 = random800_90;
            _mode = mode;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            var seedCase = GetSeedCase(@group);

            return Generate(@group, seedCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase seedCase)
        {
            MCTResult<AlgoArrayResponseWithIvs> encryptionResult = null;
            try
            {
                encryptionResult = _mode.MCTEncrypt(seedCase.Keys, seedCase.IV1, seedCase.PlainText);
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

        private TestCase GetSeedCase(TestGroup @group)
        {
            var keys = TdesHelpers.GenerateTdesKey(group.KeyingOption);
            var plainText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase
            {
                Keys = keys,
                PlainText = plainText.MSBSubstring(0, _shift),
                IV1 = iv
            };
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
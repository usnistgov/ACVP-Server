using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_CFB
{
    public class TestCaseGeneratorMonteCarloDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly int _shift;
        private readonly ICFBModeMCT _mode;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMonteCarloDecrypt(IRandom800_90 random800_90, ICFBModeMCT mode)
        {
            switch (mode.Algo)
            {
                case AlgoMode.TDES_CFB1:
                    _shift = 1;
                    break;
                case AlgoMode.TDES_CFB8:
                    _shift = 8;
                    break;
                case AlgoMode.TDES_CFB64:
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
            MCTResult<AlgoArrayResponse> decryptionResult = null;
            try
            {
                decryptionResult = _mode.MCTDecrypt(seedCase.Keys, seedCase.Iv, seedCase.CipherText);
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

        private TestCase GetSeedCase(TestGroup @group)
        {
            var keys = TdesHelpers.GenerateTdesKey(group.KeyingOption);
            var cipherText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase
            {
                Keys = keys,
                CipherText = cipherText.MSBSubstring(0, _shift),
                Iv = iv
            };
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
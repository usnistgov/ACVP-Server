using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES_CFBP;



namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestCaseGeneratorMonteCarloDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly int _shift;
        private readonly ICFBPModeMCT _mode;

        public TestCaseGeneratorMonteCarloDecrypt(IRandom800_90 random800_90, ICFBPModeMCT mode)
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
            MCTResult<AlgoArrayResponseWithIvs> decryptionResult = null;
            try
            {
                decryptionResult = _mode.MCTDecrypt(seedCase.Keys, seedCase.IV1, seedCase.CipherText);
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
            var keys = TdesHelpers.GenerateTdesKey(group.KeyingOption);
            var cipherText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
             var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase
            {
                Keys = keys,
                CipherText = cipherText.MSBSubstring(0, _shift),
                IV1 = iv
            };
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }


    }
}
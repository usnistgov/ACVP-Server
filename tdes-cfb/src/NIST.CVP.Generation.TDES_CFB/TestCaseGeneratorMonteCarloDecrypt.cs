using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES_CFB;

namespace NIST.CVP.Generation.TDES_CFB
{
    public class TestCaseGeneratorMonteCarloDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly int _shift;
        private readonly IModeOfOperationMCT _mode;

        public TestCaseGeneratorMonteCarloDecrypt(IRandom800_90 random800_90, IModeOfOperationMCT mode)
        {
            switch (mode.Algo)
            {
                case Algo.TDES_CFB1:
                    _shift = 1;
                    break;
                case Algo.TDES_CFB8:
                    _shift = 8;
                    break;
                case Algo.TDES_CFB64:
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
            MCTResult<AlgoArrayResponse> decryptionResult = null;
            try
            {
                decryptionResult = _mode.MCTDecrypt(seedCase.Key, seedCase.Iv, seedCase.CipherText);
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
            var key = TdesHelpers.GenerateTdesKey(group.KeyingOption);
            var cipherText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
             var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase
            {
                Key = key,
                CipherText = cipherText.Substring(0, _shift),
                Iv = iv
            };
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }


    }
}
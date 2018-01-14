using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES_CFB;

namespace NIST.CVP.Generation.TDES_CFB
{
    public class TestCaseGeneratorMonteCarloEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly int _shift;
        private readonly ICFBModeMCT _mode;

        public TestCaseGeneratorMonteCarloEncrypt(IRandom800_90 random800_90, ICFBModeMCT mode)
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
                encryptionResult = _mode.MCTEncrypt(seedCase.Keys, seedCase.Iv, seedCase.PlainText);
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
            var keys = TdesHelpers.GenerateTdesKey(group.KeyingOption);
            var plainText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase
            {
                Keys = keys,
                PlainText = plainText.MSBSubstring(0, _shift),
                Iv = iv
            };
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }


    }
}
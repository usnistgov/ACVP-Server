using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Helpers;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestCaseGeneratorMonteCarloDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;

        private readonly IRandom800_90 _random800_90;
        private readonly IMonteCarloTester<
            Crypto.Common.Symmetric.MCTResult<AlgoArrayResponseWithIvs>, AlgoArrayResponseWithIvs
        > _algo;
        private readonly int _shift;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMonteCarloDecrypt(TestGroup group, IRandom800_90 random800_90, IMonteCarloFactoryTdesPartitions mctFactory)
        {
            _random800_90 = random800_90;

            var mapping = AlgoModeToEngineModeOfOperationMapping.GetMapping(group.AlgoMode);
            _algo = mctFactory.GetInstance(mapping.mode);

            switch (mapping.mode)
            {
                case BlockCipherModesOfOperation.CfbpBit:
                    _shift = 1;
                    break;
                case BlockCipherModesOfOperation.CfbpByte:
                    _shift = 8;
                    break;
                case BlockCipherModesOfOperation.CfbpBlock:
                    _shift = 64;
                    break;
                default:
                    throw new ArgumentException(nameof(mapping.mode));
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            var seedCase = GetSeedCase(@group);

            return Generate(@group, seedCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase seedCase)
        {
            try
            {
                var result = _algo.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                    BlockCipherDirections.Decrypt,
                    seedCase.IV1.GetDeepCopy(),
                    seedCase.Keys.GetDeepCopy(),
                    seedCase.CipherText.GetDeepCopy()
                ));
                if (!result.Success)
                {
                    ThisLogger.Warn(result.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(result.ErrorMessage);
                    }
                }
                seedCase.ResultsArray = result.Response;
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
                }
            }
            
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
                IV1 = iv
            };
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Helpers;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Helpers;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestCaseGeneratorMmt : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly int _shift;
        private readonly BlockCipherModesOfOperation _mode;
        private int _lenGenIteration = 1;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGeneratorMmt(IOracle oracle, TestGroup group)
        {
            _oracle = oracle;
            var mapping = AlgoModeToEngineModeOfOperationMapping.GetMapping(group.AlgoMode);
            _mode = mapping.mode;

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

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            var param = new TdesParameters
            {
                Mode = _mode,
                DataLength = _lenGenIteration++ * _shift,
                Direction = group.Function,
                KeyingOption = group.KeyingOption
            };

            try
            {
                var oracleResult = await _oracle.GetTdesWithIvsCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Keys = oracleResult.Key,
                    PlainText = oracleResult.PlainText,
                    CipherText = oracleResult.CipherText,
                    IV1 = oracleResult.Iv1,
                    IV2 = oracleResult.Iv2,
                    IV3 = oracleResult.Iv3
                });
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }
    }
}

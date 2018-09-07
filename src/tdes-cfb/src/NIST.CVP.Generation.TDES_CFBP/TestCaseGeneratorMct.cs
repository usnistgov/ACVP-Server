using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Helpers;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestCaseGeneratorMct : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly int _shift;
        private readonly BlockCipherModesOfOperation _mode;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMct(IOracle oracle, TestGroup group)
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
                DataLength = _shift,
                Direction = group.Function,
                KeyingOption = group.KeyingOption
            };

            try
            {
                var oracleResult = await _oracle.GetTdesMctWithIvsCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    PlainText = oracleResult.Seed.PlainText,
                    CipherText = oracleResult.Seed.CipherText,
                    IV1 = oracleResult.Seed.Iv1,
                    Keys = oracleResult.Seed.Key,
                    ResultsArray = Array.ConvertAll(oracleResult.Results.ToArray(), element => new AlgoArrayResponseWithIvs
                    {
                        PlainText = element.PlainText,
                        CipherText = element.CipherText,
                        Keys = element.Key,
                        IV1 = element.Iv1,
                        IV2 = element.Iv2,
                        IV3 = element.Iv3,
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }
    }
}
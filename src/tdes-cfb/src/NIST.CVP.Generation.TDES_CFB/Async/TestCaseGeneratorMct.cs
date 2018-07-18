using NIST.CVP.Generation.Core;
using System;
using System.Linq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Helpers;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core.Async;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_CFB.Async
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
                case BlockCipherModesOfOperation.CfbBit:
                    _shift = 1;
                    break;
                case BlockCipherModesOfOperation.CfbByte:
                    _shift = 8;
                    break;
                case BlockCipherModesOfOperation.CfbBlock:
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
                var oracleResult = await _oracle.GetTdesMctCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    PlainText = oracleResult.Results[0].PlainText,
                    CipherText = oracleResult.Results[0].CipherText,
                    Keys = oracleResult.Results[0].Key,
                    Iv = oracleResult.Results[0].Iv,
                    ResultsArray = Array.ConvertAll(oracleResult.Results.ToArray(), element => new AlgoArrayResponse
                    {
                        PlainText = element.PlainText,
                        CipherText = element.CipherText,
                        Keys = element.Key,
                        IV = element.Iv
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
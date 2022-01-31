using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_ECB.v1_0
{
    public class TestCaseGeneratorMmt : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private const int NUMBER_OF_CASES = 10;
        private const int LENGTH_MULTIPLIER = 64;

        private int _lenGenIteration = 1;
        private readonly IOracle _oracle;

        public TestCaseGeneratorMmt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public int NumberOfTestCasesToGenerate => NUMBER_OF_CASES;

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new TdesParameters
            {
                Mode = BlockCipherModesOfOperation.Ecb,
                DataLength = _lenGenIteration++ * LENGTH_MULTIPLIER,
                Direction = group.Function,
                KeyingOption = group.KeyingOption
            };

            try
            {
                var oracleResult = await _oracle.GetTdesCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    PlainText = oracleResult.PlainText,
                    CipherText = oracleResult.CipherText
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

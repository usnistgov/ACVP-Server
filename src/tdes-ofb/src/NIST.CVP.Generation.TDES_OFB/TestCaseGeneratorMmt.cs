using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using System;

namespace NIST.CVP.Generation.TDES_OFB
{
    public class TestCaseGeneratorMmt : ITestCaseGenerator<TestGroup, TestCase>
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

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var param = new TdesParameters
            {
                Mode = BlockCipherModesOfOperation.Ofb,
                DataLength = _lenGenIteration++ * LENGTH_MULTIPLIER,
                Direction = group.Function,
                KeyingOption = TdesHelpers.GetKeyingOptionFromNumberOfKeys(group.NumberOfKeys)
            };

            try
            {
                var oracleResult = _oracle.GetTdesCase(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    PlainText = oracleResult.PlainText,
                    CipherText = oracleResult.CipherText,
                    Iv = oracleResult.Iv
                });
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            throw new NotImplementedException();
        }
    }
}

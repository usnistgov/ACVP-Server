using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using System;
using System.Linq;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestCaseGeneratorMct : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMct(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var param = new AesParameters
            {
                Mode = BlockCipherModesOfOperation.Ecb,
                DataLength = 128,
                Direction = group.Function,
                KeyLength = group.KeyLength
            };

            MctResult<AesResult> oracleResult = null;
            try
            {
                oracleResult = _oracle.GetAesMctCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                PlainText = oracleResult.Results[0].PlainText,
                CipherText = oracleResult.Results[0].CipherText,
                Key = oracleResult.Results[0].Key,
                ResultsArray = Array.ConvertAll(oracleResult.Results.ToArray(), element => new AlgoArrayResponse
                {
                    PlainText = element.PlainText,
                    CipherText = element.CipherText,
                    Key = element.Key
                }).ToList()
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }
    }
}

using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            // In instances like 2^16 aadLength, we only want to do a single test case.
            if (group.AADLength > 32 * 8)
            {
                NumberOfTestCasesToGenerate = 1;
            }

            var param = new AeadParameters
            {
                KeyLength = group.KeyLength,
                AadLength = group.AADLength,
                IvLength = group.IVLength,
                DataLength = group.PTLength,
                TagLength = group.TagLength
            };

            AeadResult oracleResult = null;
            try
            {
                oracleResult = _oracle.GetAesCcmCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                AAD = oracleResult.Aad,
                CipherText = oracleResult.CipherText,
                IV = oracleResult.Iv,
                Key = oracleResult.Key,
                PlainText = oracleResult.PlainText,
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

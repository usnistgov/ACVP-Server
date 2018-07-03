using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseGeneratorExternalEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 15;

        public TestCaseGeneratorExternalEncrypt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var param = new AeadParameters
            {
                DataLength = group.PTLength,
                KeyLength = group.KeyLength,
                AadLength = group.AADLength,
                TagLength = group.TagLength,
                IvLength = group.IVLength,
                CouldFail = false
            };

            AeadResult oracleResult = null;
            try
            {
                oracleResult = _oracle.GetAesGcmCase(param);
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
                Tag = oracleResult.Tag,
                TestPassed = oracleResult.TestPassed
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

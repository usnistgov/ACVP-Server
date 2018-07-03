using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseGeneratorInternalEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 15;

        public TestCaseGeneratorInternalEncrypt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var param = new AeadParameters
            {
                AadLength = group.AADLength,
                KeyLength = group.KeyLength,
                DataLength = group.PTLength,
                CouldFail = false
            };

            AeadResult oracleResult = null;
            try
            {
                if (isSample)
                {
                    // Get complete test case, we need all the information
                    param.TagLength = group.TagLength;
                    param.IvLength = group.IVLength;

                    oracleResult = _oracle.GetAesGcmCase(param);
                }
                else
                {
                    // Get incomplete test case, no need to compute the rest
                    oracleResult = _oracle.GetDeferredAesGcmCase(param);
                }
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

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

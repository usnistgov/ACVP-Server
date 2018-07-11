using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.AES_XPN
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
                ExternalIv = group.IVGeneration.ToLower() == "external",
                ExternalSalt = group.SaltGen.ToLower() == "external",
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
                    param.SaltLength = group.SaltLength;

                    oracleResult = _oracle.GetAesXpnCase(param);
                }
                else
                {
                    // Get incomplete test case, no need to compute the rest
                    oracleResult = _oracle.GetDeferredAesXpnCase(param);
                }
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Deferred = true,
                AAD = oracleResult.Aad,
                CipherText = oracleResult.CipherText,
                IV = oracleResult.Iv,
                Salt = oracleResult.Salt,
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

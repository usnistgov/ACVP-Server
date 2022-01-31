using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XPN.v1_0
{
    public class TestCaseGeneratorInternalEncrypt : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 15;

        public TestCaseGeneratorInternalEncrypt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new AeadParameters
            {
                ExternalIv = group.IvGeneration.ToLower() == "external",
                ExternalSalt = group.SaltGen.ToLower() == "external",
                AadLength = group.AadLength,
                KeyLength = group.KeyLength,
                PayloadLength = group.PayloadLength,
                CouldFail = false,
                TagLength = group.TagLength,
                SaltLength = group.SaltLength,    // Salt length is always 96 bits
                IvLength = group.IvLength         // Iv length is always 96 bits
            };

            try
            {
                AeadResult oracleResult = null;

                if (isSample)
                {
                    oracleResult = await _oracle.GetAesXpnCaseAsync(param);
                }
                else
                {
                    // Get incomplete test case, no need to compute the rest
                    oracleResult = await _oracle.GetDeferredAesXpnCaseAsync(param);
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
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

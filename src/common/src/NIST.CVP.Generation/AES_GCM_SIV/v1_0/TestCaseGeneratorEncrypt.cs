using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.AES_GCM_SIV.v1_0
{
    public class TestCaseGeneratorEncrypt : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        public int NumberOfTestCasesToGenerate => 15;

        public TestCaseGeneratorEncrypt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            var param = new AeadParameters
            {
                PayloadLength = group.PayloadLength,
                KeyLength = group.KeyLength,
                AadLength = group.AadLength,
                CouldFail = false
            };

            try
            {
                var oracleResult = await _oracle.GetAesGcmSivCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    AAD = oracleResult.Aad,
                    Ciphertext = oracleResult.CipherText,
                    IV = oracleResult.Iv,
                    Key = oracleResult.Key,
                    Plaintext = oracleResult.PlainText,
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

using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_XPN
{
    public class TestCaseGeneratorExternalEncrypt : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 15;

        public TestCaseGeneratorExternalEncrypt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample)
        {
            var param = new AeadParameters
            {
                DataLength = group.PTLength,
                KeyLength = group.KeyLength,
                AadLength = group.AADLength,
                SaltLength = group.SaltLength,
                TagLength = group.TagLength,
                IvLength = group.IVLength,
                CouldFail = false
            };

            try
            {
                var oracleResult = await _oracle.GetAesXpnCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
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

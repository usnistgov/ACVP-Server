using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.AES_CCM.v1_0
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            // In instances like 2^16 aadLength, we only want to do a single test case.
            if (group.AADLength > 32 * 8)
            {
                NumberOfTestCasesToGenerate = 1;
            }
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var couldFail = group.Function.ToLower() == "decrypt";

            var param = new AeadParameters
            {
                KeyLength = group.KeyLength,
                AadLength = group.AADLength,
                IvLength = group.IVLength,
                PayloadLength = group.PayloadLength,
                TagLength = group.TagLength,
                CouldFail = couldFail
            };

            try
            {
                var oracleResult = await _oracle.GetAesCcmCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    AAD = oracleResult.Aad,
                    CipherText = oracleResult.CipherText,
                    IV = oracleResult.Iv,
                    Key = oracleResult.Key,
                    PlainText = oracleResult.PlainText,
                    TestPassed = oracleResult.TestPassed
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }
        
        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.DRBG.v1_0
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 15;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            try
            {
                var oracleResult = await _oracle.GetDrbgCaseAsync(group.DrbgParameters);

                if (oracleResult.Status == DrbgStatus.Success)
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(
                        new TestCase {
                            EntropyInput = oracleResult.EntropyInput,
                            Nonce = oracleResult.Nonce,
                            OtherInput = Array.ConvertAll(oracleResult.OtherInput.ToArray(), element => new OtherInput
                            {
                                IntendedUse = element.IntendedUse,
                                AdditionalInput = element.AdditionalInput,
                                EntropyInput = element.EntropyInput
                            }).ToList(),
                            PersoString = oracleResult.PersoString,
                            ReturnedBits = oracleResult.ReturnedBits
                        }
                    );
                }

                return new TestCaseGenerateResponse<TestGroup, TestCase>(oracleResult.Status.ToString());
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


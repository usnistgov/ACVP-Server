using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.cSHAKE.v1_0
{
    public class TestCaseGeneratorMct : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public bool IsSample { get; set; } = false;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMct(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            IsSample = isSample;
            var param = new CShakeParameters
            {
                HashFunction = new HashFunction(group.DigestSize, group.DigestSize * 2),
                MessageLength = group.DigestSize,
                OutLens = group.OutputLength.GetDeepCopy(),
                IsSample = isSample
            };

            try
            {
                var oracleResult = await _oracle.GetCShakeMctCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Seed.Message,
                    Customization = oracleResult.Seed.Customization,
                    FunctionName = oracleResult.Seed.FunctionName,
                    ResultsArray = oracleResult.Results.ConvertAll(element => new AlgoArrayResponseWithCustomization
                    {
                        Message = element.Message,
                        Digest = element.Digest,
                        Customization = element.Customization,
                        FunctionName = element.FunctionName
                    })
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

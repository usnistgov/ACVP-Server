using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.CSHAKE.v1_0
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

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            IsSample = isSample;
            var param = new CShakeParameters
            {
                HashFunction = new HashFunction(group.DigestSize, group.DigestSize * 2),
                MessageLength = group.DigestSize,
                OutLens = group.OutputLength.GetDeepCopy()
            };

            try
            {
                var oracleResult = await _oracle.GetCShakeMctCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Seed.Message,
                    ResultsArray = oracleResult.Results.ConvertAll(element => new AlgoArrayResponseWithCustomization { Message = element.Message, Digest = element.Digest, Customization = element.Customization })
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

using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.KeyVer
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();

        public int NumberOfTestCasesToGenerate { get; private set; } = 12;

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            NumberOfTestCasesToGenerate = group.TestCaseExpectationProvider.ExpectationCount;
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new EddsaKeyParameters
            {
                Curve = group.Curve,
                Disposition = group.TestCaseExpectationProvider.GetRandomReason().GetReason()
            };

            try
            {
                var result = await _oracle.GetEddsaKeyVerifyAsync(param);

                var testCase = new TestCase
                {
                    Reason = param.Disposition,
                    KeyPair = result.VerifiedValue.Key,
                    TestPassed = result.Result
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating key: {ex.Message}");
            }
        }
    }
}

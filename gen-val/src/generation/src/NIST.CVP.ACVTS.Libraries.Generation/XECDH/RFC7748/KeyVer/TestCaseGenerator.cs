using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyVer
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        // Assuming that the sum of dispositions in the TestCaseExpectationProvider is equal to this variable
        public int NumberOfTestCasesToGenerate { get; private set; } = 16;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new XecdhKeyParameters
            {
                Curve = group.Curve,
                Disposition = group.TestCaseExpectationProvider.GetRandomReason()
            };

            // Disposition MsbSet does not apply to Curve448 as the public key bit size divisible by 8
            if (param.Curve == Curve.Curve448 && param.Disposition == XecdhKeyDisposition.MsbSet)
            {
                param.Disposition = XecdhKeyDisposition.None;
            }

            try
            {
                var result = await _oracle.GetXecdhKeyVerifyAsync(param);

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

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

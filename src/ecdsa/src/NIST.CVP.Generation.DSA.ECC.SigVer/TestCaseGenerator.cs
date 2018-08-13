using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.ECC.SigVer
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 15;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;
            }

            var param = new EcdsaSignatureParameters
            {
                Curve = group.Curve,
                Disposition = group.TestCaseExpectationProvider.GetRandomReason().GetReason(),
                HashAlg = group.HashAlg,
                Key = group.KeyPair
            };

            try
            {
                var result = await _oracle.GetEcdsaVerifyResultAsync(param);

                var testCase = new TestCase
                {
                    Message = result.VerifiedValue.Message,
                    KeyPair = result.VerifiedValue.Key,
                    Reason = param.Disposition,
                    TestPassed = result.Result
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating case: {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}


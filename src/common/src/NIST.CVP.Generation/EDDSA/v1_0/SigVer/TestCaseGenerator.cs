using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.EDDSA.v1_0.SigVer
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

            var keyParam = new EddsaKeyParameters
            {
                Curve = group.Curve
            };

            EddsaKeyResult keyResult = null;
            try
            {
                keyResult = await _oracle.GetEddsaKeyAsync(keyParam);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Unable to generate key");
            }

            var param = new EddsaSignatureParameters
            {
                Curve = group.Curve,
                Disposition = group.TestCaseExpectationProvider.GetRandomReason().GetReason(),
                PreHash = group.PreHash,
                Key = keyResult.Key
            };

            try
            {
                var result = await _oracle.GetEddsaVerifyResultAsync(param);

                var testCase = new TestCase
                {
                    Message = result.VerifiedValue.Message,
                    Context = result.VerifiedValue.Context,
                    KeyPair = result.VerifiedValue.Key,
                    Signature = result.VerifiedValue.Signature,
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


using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;
using System;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.Generation.ECDSA.v1_0.SigVer
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 15;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;
            }

            var keyParam = new EcdsaKeyParameters
            {
                Curve = group.Curve
            };

            EcdsaKeyResult keyResult = null;
            try
            {
                keyResult = await _oracle.GetEcdsaKeyAsync(keyParam);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Unable to generate key");
            }

            try
            {
                var param = new EcdsaSignatureParameters
                {
                    Curve = group.Curve,
                    Disposition = group.TestCaseExpectationProvider.GetRandomReason().GetReason(),
                    HashAlg = group.HashAlg,
                    Key = keyResult.Key,
                    IsMessageRandomized = group.IsMessageRandomized,
                    PreHashedMessage = group.Component,
                    NonceProviderType = NonceProviderTypes.Random        // Always the case for FIPS 186-4, choice for 186-5, but DetECDSA is handled in a separate set of gen/vals
                };

                var result = await _oracle.GetEcdsaVerifyResultAsync(param);

                var testCase = new TestCase
                {
                    Message = result.VerifiedValue.Message,
                    RandomValue = result.VerifiedValue.RandomValue,
                    RandomValueLen = result.VerifiedValue.RandomValue?.BitLength ?? 0,
                    KeyPair = result.VerifiedValue.Key,
                    Reason = param.Disposition,
                    TestPassed = result.Result,
                    Signature = result.VerifiedValue.Signature
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


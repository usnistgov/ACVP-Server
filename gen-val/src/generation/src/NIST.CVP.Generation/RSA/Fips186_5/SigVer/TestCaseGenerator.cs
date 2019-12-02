using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.RSA.Fips186_5.SigVer.TestCaseExpectations;
using NLog;

namespace NIST.CVP.Generation.RSA.Fips186_5.SigVer
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 6;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new RsaSignatureParameters
            {
                HashAlg = group.HashAlg,
                Modulo = group.Modulo,
                PaddingScheme = group.Mode,
                Reason = group.TestCaseExpectationProvider.GetRandomReason().GetReason(),
                SaltLength = group.SaltLen,
                Key = group.Key,
                MaskFunction = group.MaskFunction,
                IsMessageRandomized = group.IsMessageRandomized
            };

            try
            {
                var result = await _oracle.GetRsaVerifyAsync(param);

                var testCase = new TestCase
                {
                    Message = result.VerifiedValue.Message,
                    RandomValue = result.VerifiedValue.RandomValue,
                    RandomValueLen = result.VerifiedValue.RandomValue?.BitLength ?? 0,
                    Reason = new TestCaseExpectationReason(param.Reason),
                    TestPassed = result.Result,
                    Salt = result.VerifiedValue.Salt,
                    Signature = result.VerifiedValue.Signature?.PadToModulusMsb(group.Modulo)
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
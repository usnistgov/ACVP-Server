using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.RSA_SigVer.TestCaseExpectations;
using NLog;
using System;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 6;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var param = new RsaSignatureParameters
            {
                HashAlg = group.HashAlg,
                Modulo = group.Modulo,
                PaddingScheme = group.Mode,
                Reason = group.TestCaseExpectationProvider.GetRandomReason().GetReason(),
                SaltLength = group.SaltLen,
                Key = group.Key
            };

            VerifyResult<RsaSignatureResult> result = null;
            try
            {
                result = _oracle.GetRsaVerify(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            var testCase = new TestCase
            {
                Message = result.VerifiedValue.Message,
                Reason = new TestCaseExpectationReason(param.Reason),
                TestPassed = result.Result,
                Salt = result.VerifiedValue.Salt,
                Signature = result.VerifiedValue.Signature
            };

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

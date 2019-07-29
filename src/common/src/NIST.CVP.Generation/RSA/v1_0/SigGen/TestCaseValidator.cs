using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.RSA.v1_0.SigGen
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestGroup _serverGroup;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, VerifyResult> _deferredTestCaseResolver;
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(
            TestCase expectedResult,
            TestGroup serverGroup,
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, VerifyResult> resolver)
        {
            _serverGroup = serverGroup;
            _deferredTestCaseResolver = resolver;
            _expectedResult = expectedResult;
        }

        public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            Dictionary<string, string> expected = new Dictionary<string, string>(); ;
            Dictionary<string, string> provided = new Dictionary<string, string>(); ;

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                await CheckResults(suppliedResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join(";", errors),
                    Expected = showExpected ? expected : null,
                    Provided = showExpected ? provided : null
                };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.Signature == null)
            {
                errors.Add("Could not find r or s");
            }

            if (_serverGroup.IsMessageRandomized && suppliedResult.RandomValue == null)
            {
                errors.Add($"{nameof(suppliedResult.RandomValue)} was not supplied.");
            }

            if (_serverGroup.IsMessageRandomized && suppliedResult.RandomValueLen == 0)
            {
                errors.Add($"{nameof(suppliedResult.RandomValueLen)} was not supplied.");
            }
        }

        private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            var verifyResult = await _deferredTestCaseResolver.CompleteDeferredCryptoAsync(_serverGroup, _expectedResult, suppliedResult);
            if (!verifyResult.Success)
            {
                errors.Add($"Validation failed: {verifyResult.ErrorMessage}");
            }
        }
    }
}

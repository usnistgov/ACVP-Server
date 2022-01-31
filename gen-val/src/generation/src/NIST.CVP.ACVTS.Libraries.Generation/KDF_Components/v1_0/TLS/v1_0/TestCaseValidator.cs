using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _serverTestCase;
        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidator(TestCase serverTestCase)
        {
            _serverTestCase = serverTestCase;
        }

        public Task<TestCaseValidation> ValidateAsync(TestCase iutResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(iutResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(iutResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return Task.FromResult(new TestCaseValidation
                {
                    TestCaseId = TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                });
            }

            return Task.FromResult(new TestCaseValidation { TestCaseId = TestCaseId, Result = Core.Enums.Disposition.Passed });
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.MasterSecret == null)
            {
                errors.Add($"{nameof(suppliedResult.MasterSecret)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.KeyBlock == null)
            {
                errors.Add($"{nameof(suppliedResult.KeyBlock)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_serverTestCase.MasterSecret.Equals(suppliedResult.MasterSecret))
            {
                errors.Add($"{nameof(suppliedResult.MasterSecret)} does not match");
                expected.Add(nameof(_serverTestCase.MasterSecret), _serverTestCase.MasterSecret.ToHex());
                provided.Add(nameof(suppliedResult.MasterSecret), suppliedResult.MasterSecret.ToHex());
            }

            if (!_serverTestCase.KeyBlock.Equals(suppliedResult.KeyBlock))
            {
                errors.Add($"{nameof(suppliedResult.KeyBlock)} does not match");
                expected.Add(nameof(_serverTestCase.KeyBlock), _serverTestCase.KeyBlock.ToHex());
                provided.Add(nameof(suppliedResult.KeyBlock), suppliedResult.KeyBlock.ToHex());
            }
        }
    }
}

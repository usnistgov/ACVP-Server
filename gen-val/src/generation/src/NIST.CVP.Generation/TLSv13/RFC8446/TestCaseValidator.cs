using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.TLSv13.RFC8446
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
            if (suppliedResult.ExporterMasterSecret == null)
            {
                errors.Add($"{nameof(suppliedResult.ExporterMasterSecret)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_serverTestCase.ExporterMasterSecret.Equals(suppliedResult.ExporterMasterSecret))
            {
                errors.Add($"{nameof(suppliedResult.ExporterMasterSecret)} does not match");
                expected.Add(nameof(_serverTestCase.ExporterMasterSecret), _serverTestCase.ExporterMasterSecret.ToHex());
                provided.Add(nameof(suppliedResult.ExporterMasterSecret), suppliedResult.ExporterMasterSecret.ToHex());
            }
        }
    }
}
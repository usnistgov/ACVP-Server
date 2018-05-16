using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TLS
{
    public class TestCaseValidator : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _serverTestCase;
        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidator(TestCase serverTestCase)
        {
            _serverTestCase = serverTestCase;
        }

        public TestCaseValidation Validate(TestCase iutResult, bool showExpected = false)
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
                return new TestCaseValidation 
                { 
                    TestCaseId = TestCaseId, 
                    Result = Core.Enums.Disposition.Failed, 
                    Reason = string.Join("; ", errors),
                    Expected = showExpected ? expected : null,
                    Provided = showExpected ? provided : null
                };
            }

            return new TestCaseValidation { TestCaseId = TestCaseId, Result = Core.Enums.Disposition.Passed };
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

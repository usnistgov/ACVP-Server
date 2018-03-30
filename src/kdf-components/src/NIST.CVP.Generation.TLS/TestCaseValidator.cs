using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.TLS
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _serverTestCase;
        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidator(TestCase serverTestCase)
        {
            _serverTestCase = serverTestCase;
        }

        public TestCaseValidation Validate(TestCase iutResult)
        {
            var errors = new List<string>();

            ValidateResultPresent(iutResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(iutResult, errors);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join("; ", errors) };
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

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            if (!_serverTestCase.MasterSecret.Equals(suppliedResult.MasterSecret))
            {
                errors.Add($"{nameof(suppliedResult.MasterSecret)} does not match");
            }

            if (!_serverTestCase.KeyBlock.Equals(suppliedResult.KeyBlock))
            {
                errors.Add($"{nameof(suppliedResult.KeyBlock)} does not match");
            }
        }
    }
}

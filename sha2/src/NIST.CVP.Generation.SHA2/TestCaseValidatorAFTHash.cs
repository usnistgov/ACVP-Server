using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseValidatorAFTHash : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;

        public TestCaseValidatorAFTHash(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId
        {
            get { return _expectedResult.TestCaseId; }
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();
            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(suppliedResult, errors);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation {TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = string.Join("; ", errors)};
            }

            return new TestCaseValidation {TestCaseId = suppliedResult.TestCaseId, Result = "passed"};
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.Digest == null)
            {
                errors.Add($"{nameof(suppliedResult.Digest)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            if (!_expectedResult.Digest.Equals(suppliedResult.Digest))
            {
                errors.Add("Digests do not match");
            }
        }
    }
}

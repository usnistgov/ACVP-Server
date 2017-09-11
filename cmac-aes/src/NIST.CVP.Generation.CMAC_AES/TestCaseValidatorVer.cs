using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC_AES
{
    public class TestCaseValidatorVer : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;

        public TestCaseValidatorVer(TestCase expectedResult)
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
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = string.Join("; ", errors) };
            }
            return  new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "passed"};
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.Result == null)
            {
                errors.Add($"{nameof(suppliedResult.Result)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            if (!_expectedResult.Result.Equals(suppliedResult.Result))
            {
                errors.Add("Result does not match");
            }
        }
    }
}

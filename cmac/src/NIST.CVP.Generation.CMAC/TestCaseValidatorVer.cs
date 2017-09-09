using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseValidatorVer<TTestCase> : ITestCaseValidator<TTestCase>
        where TTestCase : TestCaseBase
    {
        private readonly TTestCase _expectedResult;

        public TestCaseValidatorVer(TTestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId
        {
            get { return _expectedResult.TestCaseId; }
        }

        public TestCaseValidation Validate(TTestCase suppliedResult)
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

        private void ValidateResultPresent(TTestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.Result == null)
            {
                errors.Add($"{nameof(suppliedResult.Result)} was not present in the {nameof(TTestCase)}");
            }
        }

        private void CheckResults(TTestCase suppliedResult, List<string> errors)
        {
            if (!_expectedResult.Result.Equals(suppliedResult.Result))
            {
                errors.Add("Result does not match");
            }
        }
    }
}

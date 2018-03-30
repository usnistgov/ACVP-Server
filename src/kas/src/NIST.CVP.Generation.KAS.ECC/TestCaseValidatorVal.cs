using System;
using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestCaseValidatorVal : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;

        public TestCaseValidatorVal(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

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
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join("; ", errors) };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (string.IsNullOrEmpty(suppliedResult.Result))
            {
                errors.Add($"{nameof(suppliedResult.Result)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            if (_expectedResult.FailureTest)
            {
                if (!suppliedResult.Result.Equals("fail", StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add("Expected failure test.");
                }
            }
            else
            {
                if (!suppliedResult.Result.Equals("pass", StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add("Expected pass test.");
                }
            }
        }
    }
}
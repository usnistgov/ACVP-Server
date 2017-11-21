using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestCaseValidatorDecrypt : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;

        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidatorDecrypt(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
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
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join("; ", errors) };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.PlainText == null)
            {
                errors.Add($"{nameof(suppliedResult.PlainText)} was not present in the {nameof(TestCase)}");
                return;
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            if (!_expectedResult.PlainText.Equals(suppliedResult.PlainText))
            {
                errors.Add("Plain Text does not match");
            }
        }
    }
}

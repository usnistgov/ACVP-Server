using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseValidatorDecrypt : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;

        public TestCaseValidatorDecrypt(TestCase expectedResult)
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
            if (_expectedResult.FailureTest)
            {
                if (!suppliedResult.FailureTest)
                {
                    errors.Add("Expected tag validation failure");
                }
            }
            else
            {
                ValidateResultPresent(suppliedResult, errors);
                if (errors.Count == 0)
                {
                    CheckResults(suppliedResult, errors);
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Failed, Reason = string.Join("; ", errors) };
            }
            return  new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Passed };
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

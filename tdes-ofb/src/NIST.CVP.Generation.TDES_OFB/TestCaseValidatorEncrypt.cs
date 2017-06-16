using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_OFB
{
    public class TestCaseValidatorEncrypt : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;

        public TestCaseValidatorEncrypt(TestCase expectedResult)
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

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "passed" };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.CipherText == null)
            {
                errors.Add($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            if (!_expectedResult.CipherText.Equals(suppliedResult.CipherText))
            {
                errors.Add("Cipher Texts do not match");
            }
        }
    }
}

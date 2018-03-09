using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestCaseValidatorEncrypt : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorEncrypt(TestCase expectedResult)
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
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Failed, Reason = string.Join("; ", errors) };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.CipherText == null)
            {
                if (suppliedResult.CipherText1 == null && suppliedResult.CipherText2 == null && suppliedResult.CipherText3 == null)
                {
                    errors.Add($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}");
                }
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            if (_expectedResult.CipherText != null && suppliedResult.CipherText != null)
            {
                if (!_expectedResult.CipherText.Equals(suppliedResult.CipherText))
                {
                    errors.Add("Cipher Texts do not match");
                }
            }

            if (suppliedResult.CipherText1 != null && suppliedResult.CipherText2 != null && suppliedResult.CipherText3 != null &&
                _expectedResult.CipherText1 != null && _expectedResult.CipherText2 != null && _expectedResult.CipherText3 != null)
            {
                if (!_expectedResult.CipherText1.Equals(suppliedResult.CipherText1))
                {
                    errors.Add("Cipher Texts 1 do not match");
                }

                if (!_expectedResult.CipherText2.Equals(suppliedResult.CipherText2))
                {
                    errors.Add("Cipher Texts 2 do not match");
                }

                if (!_expectedResult.CipherText3.Equals(suppliedResult.CipherText3))
                {
                    errors.Add("Cipher Texts 3 do not match");
                }
            }
        }
    }
}

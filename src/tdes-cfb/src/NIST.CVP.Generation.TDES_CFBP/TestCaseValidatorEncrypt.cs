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

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(suppliedResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation 
                { 
                    TestCaseId = suppliedResult.TestCaseId, 
                    Result = Disposition.Failed, 
                    Reason = string.Join("; ", errors),
                    Expected = showExpected ? expected : null,
                    Provided = showExpected ? provided : null
                };
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

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (_expectedResult.CipherText != null && suppliedResult.CipherText != null)
            {
                if (!_expectedResult.CipherText.Equals(suppliedResult.CipherText))
                {
                    errors.Add("Cipher Texts do not match");
                    expected.Add(nameof(_expectedResult.CipherText), _expectedResult.CipherText.ToHex());
                    provided.Add(nameof(suppliedResult.CipherText), suppliedResult.CipherText.ToHex());
                }
            }

            if (suppliedResult.CipherText1 != null && suppliedResult.CipherText2 != null && suppliedResult.CipherText3 != null &&
                _expectedResult.CipherText1 != null && _expectedResult.CipherText2 != null && _expectedResult.CipherText3 != null)
            {
                if (!_expectedResult.CipherText1.Equals(suppliedResult.CipherText1))
                {
                    errors.Add("Cipher Texts 1 do not match");
                    expected.Add(nameof(_expectedResult.CipherText1), _expectedResult.CipherText1.ToHex());
                    provided.Add(nameof(suppliedResult.CipherText1), suppliedResult.CipherText1.ToHex());
                }

                if (!_expectedResult.CipherText2.Equals(suppliedResult.CipherText2))
                {
                    errors.Add("Cipher Texts 2 do not match");
                    expected.Add(nameof(_expectedResult.CipherText2), _expectedResult.CipherText2.ToHex());
                    provided.Add(nameof(suppliedResult.CipherText2), suppliedResult.CipherText2.ToHex());
                }

                if (!_expectedResult.CipherText3.Equals(suppliedResult.CipherText3))
                {
                    errors.Add("Cipher Texts 3 do not match");
                    expected.Add(nameof(_expectedResult.CipherText3), _expectedResult.CipherText3.ToHex());
                    provided.Add(nameof(suppliedResult.CipherText3), suppliedResult.CipherText3.ToHex());
                }
            }
        }
    }
}

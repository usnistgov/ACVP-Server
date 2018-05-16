using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.TDES_CTR
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
                errors.Add($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}");
                return;
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_expectedResult.CipherText.Equals(suppliedResult.CipherText))
            {
                errors.Add("CipherText does not match");
                expected.Add(nameof(_expectedResult.CipherText), _expectedResult.CipherText.ToHex());
                provided.Add(nameof(suppliedResult.CipherText), suppliedResult.CipherText.ToHex());
            }
        }
    }
}

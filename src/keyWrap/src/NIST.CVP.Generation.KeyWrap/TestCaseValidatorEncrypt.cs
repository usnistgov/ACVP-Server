using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseValidatorEncrypt<TTestGroup, TTestCase> : ITestCaseValidator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        private readonly TTestCase _expectedResult;

        public TestCaseValidatorEncrypt(TTestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidation Validate(TTestCase suppliedResult, bool showExpected = false)
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
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = showExpected ? expected : null,
                    Provided = showExpected ? provided : null
                };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TTestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.CipherText == null)
            {
                errors.Add($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TTestCase)}");
                return;
            }
        }

        private void CheckResults(TTestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_expectedResult.CipherText.Equals(suppliedResult.CipherText))
            {
                errors.Add($"{nameof(suppliedResult.CipherText)} does not match");
            }
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestCaseValidatorDecrypt : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;

        public TestCaseValidatorDecrypt(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

        public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            if (_expectedResult.TestPassed != null && !_expectedResult.TestPassed.Value)
            {
                if (!suppliedResult.TestPassed.HasValue || 
                    (suppliedResult.TestPassed != null && suppliedResult.TestPassed.Value))
                {
                    errors.Add("Expected tag validation failure");
                    expected.Add(nameof(_expectedResult.TestPassed), _expectedResult.TestPassed.Value.ToString());
                    provided.Add(nameof(suppliedResult.TestPassed),
                        suppliedResult.TestPassed.HasValue ? suppliedResult.TestPassed.ToString() : "none");
                }
            }
            else
            {
                ValidateResultPresent(suppliedResult, errors);
                if (errors.Count == 0)
                {
                    CheckResults(suppliedResult, errors, expected, provided);
                }
            }

            if (errors.Count > 0)
            {
                return await Task.FromResult(new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId, 
                    Result = Core.Enums.Disposition.Failed, 
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                });
            }

            return await Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Core.Enums.Disposition.Passed
            });
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.PlainText == null)
            {
                errors.Add($"{nameof(suppliedResult.PlainText)} was not present in the {nameof(TestCase)}");
                return;
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_expectedResult.PlainText.Equals(suppliedResult.PlainText))
            {
                errors.Add("Plain Text does not match");
                expected.Add(nameof(_expectedResult.PlainText), _expectedResult.PlainText.ToHex());
                provided.Add(nameof(suppliedResult.PlainText), suppliedResult.PlainText.ToHex());
            }
        }
    }
}

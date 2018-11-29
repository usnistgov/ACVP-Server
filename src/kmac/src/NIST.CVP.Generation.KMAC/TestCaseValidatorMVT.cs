using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCaseValidatorMvt : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _currentGroup;

        public TestCaseValidatorMvt(TestCase expectedResult, TestGroup currentGroup)
        {
            _expectedResult = expectedResult;
            _currentGroup = currentGroup;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

        public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
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
                return Task.FromResult(new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                });
            }

            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Core.Enums.Disposition.Passed
            });
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.TestPassed == null)
            {
                errors.Add($"{nameof(suppliedResult.TestPassed)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (_expectedResult.TestPassed != suppliedResult.TestPassed)
            {
                errors.Add("MAC Verification does not match");
                expected.Add(nameof(_expectedResult.TestPassed), _expectedResult.TestPassed.ToString());
                provided.Add(nameof(suppliedResult.TestPassed), suppliedResult.TestPassed.ToString());
            }
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.OneStep
{
    public class TestCaseValidatorVal : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _workingTest;

        public TestCaseValidatorVal(TestCase workingTest)
        {
            _workingTest = workingTest;
        }

        public int TestCaseId => _workingTest.TestCaseId;

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
            { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed });
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.TestPassed == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.TestPassed)} but was not supplied");
            }
        }

        private void CheckResults(
            TestCase suppliedResult,
            List<string> errors,
            Dictionary<string, string> expected,
            Dictionary<string, string> provided)
        {
            if (!_workingTest.TestPassed.Equals(suppliedResult.TestPassed))
            {
                errors.Add($"{nameof(suppliedResult.TestPassed)} does not match");
                expected.Add(nameof(_workingTest.TestPassed), _workingTest.TestPassed.ToString());
                provided.Add(nameof(suppliedResult.TestPassed), suppliedResult.TestPassed.ToString());
            }
        }
    }
}

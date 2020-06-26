using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KAS_KDF.OneStep
{
	public class TestCaseValidatorAft : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _workingTest;

        public TestCaseValidatorAft(TestCase workingTest)
        {
            _workingTest = workingTest;
        }
    
        public int TestCaseId => _workingTest.TestCaseId;
        
        public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                await CheckResults(suppliedResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.Dkm == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.Dkm)} but was not supplied");
            }
        }
        
        private async Task CheckResults(
            TestCase suppliedResult, 
            List<string> errors, 
            Dictionary<string, string> expected, 
            Dictionary<string, string> provided)
        {
            if (!_workingTest.Dkm.Equals(suppliedResult.Dkm))
            {
                errors.Add($"{nameof(suppliedResult.Dkm)} does not match");
                expected.Add(nameof(_workingTest.Dkm), _workingTest.Dkm.ToHex());
                provided.Add(nameof(suppliedResult.Dkm), suppliedResult.Dkm.ToHex());
            }                                
        }
    }
}
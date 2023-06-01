using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(
            TestCase expectedResult
            )
        {
            _expectedResult = expectedResult;
        }

        public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
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
                return await Task.FromResult(new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                });
            }

            return await Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Disposition.Passed
            });
        }

        
        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.TestPassed == true)
            {
                if (suppliedResult.Signature == null)
                {
                    errors.Add($"{nameof(suppliedResult.Signature)} was not present in the {nameof(TestCase)}");
                }
            }
        }
        
        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (suppliedResult.TestPassed == true)
            {
                if (_expectedResult.TestPassed != true)
                {
                    errors.Add("Test was expected to fail");
                    expected.Add(nameof(_expectedResult.TestPassed), _expectedResult.TestPassed.ToString());
                    provided.Add(nameof(suppliedResult.TestPassed), suppliedResult.TestPassed.ToString());
                }
                else
                {
                    if (!_expectedResult.Signature.Equals(suppliedResult.Signature))
                    {
                        errors.Add("Signature does not match expected value");
                        expected.Add(nameof(_expectedResult.Signature), _expectedResult.Signature.ToHex());
                        provided.Add(nameof(suppliedResult.Signature), suppliedResult.Signature.ToHex());
                    }
                }
            }
            else
            {
                if (_expectedResult.TestPassed == true)
                {
                    errors.Add("Test was expected to pass");
                    expected.Add(nameof(_expectedResult.TestPassed), _expectedResult.TestPassed.ToString());
                    provided.Add(nameof(suppliedResult.TestPassed), suppliedResult.TestPassed.ToString());
                }
            }
        }
    }
}
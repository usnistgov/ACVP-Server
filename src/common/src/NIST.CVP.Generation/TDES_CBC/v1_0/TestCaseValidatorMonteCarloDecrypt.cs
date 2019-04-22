using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.TDES_CBC.v1_0
{
    public class TestCaseValidatorMonteCarloDecrypt : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorMonteCarloDecrypt(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateArrayResultPresent(suppliedResult, errors);
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

        private void ValidateArrayResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.ResultsArray == null || suppliedResult.ResultsArray.Count == 0)
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}");
                return;
            }

            if (suppliedResult.ResultsArray.Any(a => a.Keys == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Keys)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.PlainText == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.PlainText)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.CipherText == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.CipherText)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            for (var i = 0; i < _expectedResult.ResultsArray.Count; i++)
            {
                if (!_expectedResult.ResultsArray[i].Keys.Equals(suppliedResult.ResultsArray[i].Keys))
                {
                    errors.Add($"Key does not match on iteration {i}");
                    expected.Add($"Key {i}", _expectedResult.ResultsArray[i].Keys.ToHex());
                    provided.Add($"Key {i}", suppliedResult.ResultsArray[i].Keys.ToHex());
                }
                if (!_expectedResult.ResultsArray[i].CipherText.Equals(suppliedResult.ResultsArray[i].CipherText))
                {
                    errors.Add($"Cipher Text does not match on iteration {i}");
                    expected.Add($"Cipher Text {i}", _expectedResult.ResultsArray[i].CipherText.ToHex());
                    provided.Add($"Cipher Text {i}", suppliedResult.ResultsArray[i].CipherText.ToHex());
                }
                if (!_expectedResult.ResultsArray[i].PlainText.Equals(suppliedResult.ResultsArray[i].PlainText))
                {
                    errors.Add($"Plain Text does not match on iteration {i}");
                    expected.Add($"Plain Text {i}", _expectedResult.ResultsArray[i].PlainText.ToHex());
                    provided.Add($"Plain Text {i}", suppliedResult.ResultsArray[i].PlainText.ToHex());
                }
            }
        }
    }
}
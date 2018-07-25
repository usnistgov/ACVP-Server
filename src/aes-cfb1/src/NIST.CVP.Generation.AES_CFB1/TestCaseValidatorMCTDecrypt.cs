using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class TestCaseValidatorMCTDecrypt : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;

        public TestCaseValidatorMCTDecrypt(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

        public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            // Can only check the contents of the array, 
            // if the array and all expected elements within the array are available
            ValidateArrayResultPresent(suppliedResult, errors);
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

        private void ValidateArrayResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.ResultsArray == null || suppliedResult.ResultsArray.Count == 0)
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}");
                return;
            }

            if (suppliedResult.ResultsArray.Any(a => a.Key == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Key)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.IV == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.IV)}");
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
            for (int i = 0; i < _expectedResult.ResultsArray.Count; i++)
            {
                if (!_expectedResult.ResultsArray[i].IV.Equals(suppliedResult.ResultsArray[i].IV))
                {
                    errors.Add($"IV does not match on iteration {i}");
                    expected.Add($"{i} IV", _expectedResult.ResultsArray[i].IV.ToHex());
                    provided.Add($"{i} IV", suppliedResult.ResultsArray[i].IV.ToHex());
                }
                if (!_expectedResult.ResultsArray[i].Key.Equals(suppliedResult.ResultsArray[i].Key))
                {
                    errors.Add($"Key does not match on iteration {i}");
                    expected.Add($"{i} Key", _expectedResult.ResultsArray[i].Key.ToHex());
                    provided.Add($"{i} Key", suppliedResult.ResultsArray[i].Key.ToHex());
                }
                if (!_expectedResult.ResultsArray[i].CipherText.Equals(suppliedResult.ResultsArray[i].CipherText))
                {
                    errors.Add($"Cipher Text does not match on iteration {i}");
                    expected.Add($"{i} cipherText", _expectedResult.ResultsArray[i].CipherText.ToHex());
                    provided.Add($"{i} cipherText", suppliedResult.ResultsArray[i].CipherText.ToHex());
                }
                if (!_expectedResult.ResultsArray[i].PlainText.Equals(suppliedResult.ResultsArray[i].PlainText))
                {
                    errors.Add($"Plain Text does not match on iteration {i}");
                    expected.Add($"{i} plainText", _expectedResult.ResultsArray[i].PlainText.ToHex());
                    provided.Add($"{i} plainText", suppliedResult.ResultsArray[i].PlainText.ToHex());
                }
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_OFBI
{
    public class TestCaseValidatorMonteCarloDecrypt : ITestCaseValidator<TestGroup, TestCase>
    {
        private TestCase _expectedResult;

        public TestCaseValidatorMonteCarloDecrypt(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();
            ValidateArrayResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(suppliedResult, errors);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join("; ", errors) };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
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
            if (suppliedResult.ResultsArray.Any(a => a.IV1 == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithIvs.IV1)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.IV2 == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithIvs.IV2)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.IV3 == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithIvs.IV3)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            if (_expectedResult.ResultsArray.Count != suppliedResult.ResultsArray.Count)
            {
                errors.Add("Expected results and supplied results arrays sizes do not match");
                return;
            }
            for (var i = 0; i < _expectedResult.ResultsArray.Count; i++)
            {
                if (!_expectedResult.ResultsArray[i].Keys.Equals(suppliedResult.ResultsArray[i].Keys))
                {
                    errors.Add($"Key does not match on iteration {i}");
                }
                if (!_expectedResult.ResultsArray[i].CipherText.Equals(suppliedResult.ResultsArray[i].CipherText))
                {
                    errors.Add($"Cipher Text does not match on iteration {i}");
                }
                if (!_expectedResult.ResultsArray[i].PlainText.Equals(suppliedResult.ResultsArray[i].PlainText))
                {
                    errors.Add($"Plain Text does not match on iteration {i}");
                }
                if (!_expectedResult.ResultsArray[i].IV1.Equals(suppliedResult.ResultsArray[i].IV1))
                {
                    errors.Add($"IV1 does not match on iteration {i}");
                }
                if (!_expectedResult.ResultsArray[i].IV2.Equals(suppliedResult.ResultsArray[i].IV2))
                {
                    errors.Add($"IV2 does not match on iteration {i}");
                }
                if (!_expectedResult.ResultsArray[i].IV3.Equals(suppliedResult.ResultsArray[i].IV3))
                {
                    errors.Add($"IV3 does not match on iteration {i}");
                }
            }
        }
    }
}

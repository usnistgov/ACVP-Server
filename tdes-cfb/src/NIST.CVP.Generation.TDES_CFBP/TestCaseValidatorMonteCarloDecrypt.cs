using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestCaseValidatorMonteCarloDecrypt : ITestCaseValidator<TestCase>
    {
        private TestCase _expectedResult;

        public TestCaseValidatorMonteCarloDecrypt(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId
        {
            get { return _expectedResult.TestCaseId; }
        }

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
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Failed, Reason = string.Join("; ", errors) };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Passed };
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
            }
        }
    }
}

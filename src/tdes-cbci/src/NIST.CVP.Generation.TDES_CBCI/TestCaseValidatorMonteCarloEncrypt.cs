using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CBCI
{
    public class TestCaseValidatorMonteCarloEncrypt : ITestCaseValidator<TestGroup, TestCase>
    {
        private TestCase _expectedResult;

        public TestCaseValidatorMonteCarloEncrypt(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
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

        private void ValidateArrayResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.ResultsArray == null || suppliedResult.ResultsArray.Count == 0)
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}");
                return;
            }

            if (suppliedResult.ResultsArray.Any(a => a.Keys == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithIvs.Keys)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.PlainText == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithIvs.PlainText)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.CipherText == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithIvs.CipherText)}");
            }

            if (suppliedResult.ResultsArray.Any(a => a.IV1 == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithIvs.IV1)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.IV2 == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithIvs.IV3)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.IV3 == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithIvs.IV3)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
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
                if (!_expectedResult.ResultsArray[i].IV1.Equals(suppliedResult.ResultsArray[i].IV1))
                {
                    errors.Add($"IV1 does not match on iteration {i}");
                    expected.Add($"IV1 {i}", _expectedResult.ResultsArray[i].IV1.ToHex());
                    provided.Add($"IV1 {i}", suppliedResult.ResultsArray[i].IV1.ToHex());
                }
                if (!_expectedResult.ResultsArray[i].IV2.Equals(suppliedResult.ResultsArray[i].IV2))
                {
                    errors.Add($"IV2 does not match on iteration {i}");
                    expected.Add($"IV2 {i}", _expectedResult.ResultsArray[i].IV2.ToHex());
                    provided.Add($"IV2 {i}", suppliedResult.ResultsArray[i].IV2.ToHex());
                }
                if (!_expectedResult.ResultsArray[i].IV3.Equals(suppliedResult.ResultsArray[i].IV3))
                {
                    errors.Add($"IV3 does not match on iteration {i}");
                    expected.Add($"IV3 {i}", _expectedResult.ResultsArray[i].IV3.ToHex());
                    provided.Add($"IV3 {i}", suppliedResult.ResultsArray[i].IV3.ToHex());
                }
            }
        }
    }
}

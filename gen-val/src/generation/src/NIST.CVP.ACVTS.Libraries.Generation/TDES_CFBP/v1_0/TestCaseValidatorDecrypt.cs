﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CFBP.v1_0
{
    public class TestCaseValidatorDecrypt : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorDecrypt(TestCase expectedResult)
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
            if (suppliedResult.PlainText == null && suppliedResult.PlainText1 == null && suppliedResult.PlainText2 == null && suppliedResult.PlainText3 == null)
            {
                errors.Add($"{nameof(suppliedResult.PlainText)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (_expectedResult.PlainText != null && suppliedResult.PlainText != null)
            {
                if (!_expectedResult.PlainText.Equals(suppliedResult.PlainText))
                {
                    errors.Add("Plain Text does not match");
                    expected.Add(nameof(_expectedResult.PlainText), _expectedResult.PlainText.ToHex());
                    provided.Add(nameof(suppliedResult.PlainText), suppliedResult.PlainText.ToHex());
                }
            }

            if (suppliedResult.PlainText1 != null && suppliedResult.PlainText2 != null && suppliedResult.PlainText3 != null &&
                _expectedResult.PlainText1 != null && _expectedResult.PlainText2 != null && _expectedResult.PlainText3 != null)
            {
                if (!_expectedResult.PlainText1.Equals(suppliedResult.PlainText1))
                {
                    errors.Add("Plain Texts 1 do not match");
                    expected.Add(nameof(_expectedResult.PlainText1), _expectedResult.PlainText1.ToHex());
                    provided.Add(nameof(suppliedResult.PlainText1), suppliedResult.PlainText1.ToHex());
                }

                if (!_expectedResult.PlainText2.Equals(suppliedResult.PlainText2))
                {
                    errors.Add("Plain Texts 2 do not match");
                    expected.Add(nameof(_expectedResult.PlainText2), _expectedResult.PlainText2.ToHex());
                    provided.Add(nameof(suppliedResult.PlainText2), suppliedResult.PlainText2.ToHex());
                }

                if (!_expectedResult.PlainText3.Equals(suppliedResult.PlainText3))
                {
                    errors.Add("Plain Texts 3 do not match");
                    expected.Add(nameof(_expectedResult.PlainText3), _expectedResult.PlainText3.ToHex());
                    provided.Add(nameof(suppliedResult.PlainText3), suppliedResult.PlainText3.ToHex());
                }
            }
        }
    }
}

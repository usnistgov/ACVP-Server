﻿using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseValidatorDecrypt<TTestGroup, TTestCase> : ITestCaseValidator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        private readonly TTestCase _expectedResult;

        public TestCaseValidatorDecrypt(TTestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidation Validate(TTestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            if (_expectedResult.TestPassed.Value)
            {
                if (!suppliedResult.TestPassed.Value)
                {
                    errors.Add("Expected tag validation failure");
                    expected.Add(nameof(_expectedResult.TestPassed), _expectedResult.TestPassed.Value.ToString());
                    provided.Add(nameof(suppliedResult.TestPassed), suppliedResult.TestPassed.Value.ToString());
                }
            }
            else
            {
                ValidateResultPresent(suppliedResult, errors);
                if (errors.Count == 0)
                {
                    CheckResults(suppliedResult, errors, expected, provided);
                }
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

        private void ValidateResultPresent(TTestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.PlainText == null)
            {
                errors.Add($"{nameof(suppliedResult.PlainText)} was not present in the {nameof(TTestCase)}");
                return;
            }
        }

        private void CheckResults(TTestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_expectedResult.PlainText.Equals(suppliedResult.PlainText))
            {
                errors.Add($"{nameof(suppliedResult.PlainText)} does not match");
                expected.Add(nameof(_expectedResult.PlainText), _expectedResult.PlainText.ToHex());
                provided.Add(nameof(suppliedResult.PlainText), suppliedResult.PlainText.ToHex());
            }
        }
    }
}
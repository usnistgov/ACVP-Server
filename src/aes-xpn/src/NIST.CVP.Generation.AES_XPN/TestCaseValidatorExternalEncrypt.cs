﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.AES_XPN
{
    public class TestCaseValidatorExternalEncrypt : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;

        public TestCaseValidatorExternalEncrypt(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

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
                    Result = Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                });
            }

            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Disposition.Passed
            });
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.CipherText == null)
            {
                errors.Add($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}");
            }
            if (suppliedResult.Tag == null)
            {
                errors.Add($"{nameof(suppliedResult.Tag)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_expectedResult.CipherText.Equals(suppliedResult.CipherText))
            {
                errors.Add("Cipher Text does not match");
                expected.Add(nameof(_expectedResult.CipherText), _expectedResult.CipherText.ToHex());
                provided.Add(nameof(suppliedResult.CipherText), suppliedResult.CipherText.ToHex());
            }
            if (!_expectedResult.Tag.Equals(suppliedResult.Tag))
            {
                errors.Add("Tag does not match");
                expected.Add(nameof(_expectedResult.Tag), _expectedResult.Tag.ToHex());
                provided.Add(nameof(suppliedResult.Tag), suppliedResult.Tag.ToHex());
            }
        }
    }
}

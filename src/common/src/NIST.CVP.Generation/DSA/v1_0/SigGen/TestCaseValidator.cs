﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.v1_0.SigGen
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _group;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, FfcVerificationResult> _deferredResolver;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(
            TestCase expectedResult, 
            TestGroup group, 
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, FfcVerificationResult> deferredResolver
        )
        {
            _expectedResult = expectedResult;
            _group = group;
            _deferredResolver = deferredResolver;
        }

        public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            Dictionary<string, string> expected = null;
            Dictionary<string, string> provided = null;

            if (suppliedResult.Signature == null)
            {
                errors.Add("Could not find r or s");
            }
            else
            {
                var verifyResult = await _deferredResolver.CompleteDeferredCryptoAsync(_group, _expectedResult, suppliedResult);
                if (!verifyResult.Success)
                {
                    errors.Add($"Validation failed: {verifyResult.ErrorMessage}");
                    expected = new Dictionary<string, string>();
                    provided = new Dictionary<string, string>();
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join(";", errors),
                    Expected = showExpected ? expected : null,
                    Provided = showExpected ? provided : null
                };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }
    }
}
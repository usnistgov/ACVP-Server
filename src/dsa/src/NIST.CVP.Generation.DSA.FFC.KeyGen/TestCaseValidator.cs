﻿using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
{
    public class TestCaseValidator : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _group;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, FfcKeyPairValidateResult> _deferredResolver;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(TestCase expectedResult, TestGroup group, IDeferredTestCaseResolver<TestGroup, TestCase, FfcKeyPairValidateResult> deferredResolver)
        {
            _expectedResult = expectedResult;
            _group = group;
            _deferredResolver = deferredResolver;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            Dictionary<string, string> expected = null;
            Dictionary<string, string> provided = null;

            if (suppliedResult.Key.PrivateKeyX == 0 || suppliedResult.Key.PublicKeyY == 0)
            {
                errors.Add("Could not find x or y");
            }
            else
            {
                var validateResult = _deferredResolver.CompleteDeferredCrypto(_group, _expectedResult, suppliedResult);
                if (!validateResult.Success)
                {
                    errors.Add($"Validation failed: {validateResult.ErrorMessage}");
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
﻿using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseValidatorDeferredEncrypt : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestGroup _testGroup;
        private readonly TestCase _serverTestCase;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCipherAeadResult> _testCaseResolver;

        public TestCaseValidatorDeferredEncrypt(TestGroup testGroup, TestCase serverTestCase, IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCipherAeadResult> deferredTestCaseResolver)
        {
            _testGroup = testGroup;
            _serverTestCase = serverTestCase;
            _testCaseResolver = deferredTestCaseResolver;
        }

        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
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
                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId, 
                    Result = Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Passed };
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
            var serverResult = _testCaseResolver.CompleteDeferredCrypto(_testGroup, _serverTestCase, suppliedResult);

            if (!serverResult.Result.Equals(suppliedResult.CipherText))
            {
                errors.Add("Cipher Text does not match");
                expected.Add(nameof(serverResult.Result), serverResult.Result.ToHex());
                provided.Add(nameof(suppliedResult.CipherText), suppliedResult.CipherText.ToHex());
            }
            if (!serverResult.Tag.Equals(suppliedResult.Tag))
            {
                errors.Add("Tag does not match");
                expected.Add(nameof(serverResult.Tag), serverResult.Tag.ToHex());
                provided.Add(nameof(suppliedResult.Tag), suppliedResult.Tag.ToHex());
            }
        }
    }
}
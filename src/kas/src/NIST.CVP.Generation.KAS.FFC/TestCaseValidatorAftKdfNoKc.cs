﻿using System.Collections.Generic;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseValidatorAftKdfNoKc : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _workingResult;
        private readonly TestGroup _testGroup;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, KasResult> _deferredResolver;

        private readonly SchemeKeyNonceGenRequirement<FfcScheme> _iutKeyRequirements;

        public TestCaseValidatorAftKdfNoKc(TestCase workingResult, TestGroup testGroup, IDeferredTestCaseResolver<TestGroup, TestCase, KasResult> deferredResolver)
        {
            _workingResult = workingResult;
            _testGroup = testGroup;
            _deferredResolver = deferredResolver;

            _iutKeyRequirements =
                KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                    _testGroup.Scheme,
                    _testGroup.KasMode,
                    _testGroup.KasRole,
                    _testGroup.KcRole,
                    _testGroup.KcType
                );
        }

        public int TestCaseId => _workingResult.TestCaseId;

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
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (_iutKeyRequirements.GeneratesStaticKeyPair)
            {
                if (suppliedResult.StaticPublicKeyIut == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.StaticPublicKeyIut)} but was not supplied");
                }
            }

            if (_iutKeyRequirements.GeneratesEphemeralKeyPair)
            {
                if (suppliedResult.EphemeralPublicKeyIut == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.EphemeralPublicKeyIut)} but was not supplied");
                }
            }

            if (_iutKeyRequirements.GeneratesDkmNonce)
            {
                if (suppliedResult.DkmNonceIut == null || suppliedResult.DkmNonceIut.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.DkmNonceIut)} but was not supplied");
                }
            }

            // AES-CCM nonce required only when IUT is both initiator, and macType is AES-CCM
            if (_testGroup.KasRole == KeyAgreementRole.InitiatorPartyU
                && _testGroup.MacType == KeyAgreementMacType.AesCcm)
            {
                if (suppliedResult.NonceAesCcm == null || suppliedResult.NonceAesCcm.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.NonceAesCcm)} but was not supplied");
                }
            }

            if (suppliedResult.IdIutLen == 0)
            {
                errors.Add($"Expected {nameof(suppliedResult.IdIutLen)} must be supplied and non zero");
            }
            if (suppliedResult.IdIut == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.IdIut)} but was not supplied");
            }
            
            if (suppliedResult.OiLen == 0)
            {
                errors.Add($"Expected {nameof(suppliedResult.OiLen)} must be supplied and non zero");
            }
            if (suppliedResult.OtherInfo == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.OtherInfo)} but was not supplied");
            }
            
            if (suppliedResult.Tag == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.Tag)} but was not supplied");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            KasResult serverResult = _deferredResolver.CompleteDeferredCrypto(_testGroup, _workingResult, suppliedResult);

            if (!serverResult.Tag.Equals(suppliedResult.Tag))
            {
                errors.Add($"{nameof(suppliedResult.Tag)} does not match");
                expected.Add(nameof(serverResult.Tag), serverResult.Tag.ToHex());
                provided.Add(nameof(suppliedResult.Tag), suppliedResult.Tag.ToHex());
            }
        }
    }
}
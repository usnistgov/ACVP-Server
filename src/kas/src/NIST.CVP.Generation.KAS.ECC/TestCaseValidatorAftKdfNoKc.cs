using System.Collections.Generic;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestCaseValidatorAftKdfNoKc : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _workingResult;
        private readonly TestGroup _testGroup;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, KasResult> _deferredResolver;

        private readonly SchemeKeyNonceGenRequirement<EccScheme> _iutKeyRequirements;

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

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            ValidateResultPresent(suppliedResult, errors);
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

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (_iutKeyRequirements.GeneratesStaticKeyPair)
            {
                if (suppliedResult.StaticPublicKeyIutX == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.StaticPublicKeyIutX)} but was not supplied");
                }
                if (suppliedResult.StaticPublicKeyIutY == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.StaticPublicKeyIutY)} but was not supplied");
                }
            }
            
            if (_iutKeyRequirements.GeneratesEphemeralKeyPair)
            {
                if (suppliedResult.EphemeralPublicKeyIutX == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.EphemeralPublicKeyIutX)} but was not supplied");
                }
                if (suppliedResult.EphemeralPublicKeyIutY == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.EphemeralPublicKeyIutY)} but was not supplied");
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

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            KasResult serverResult = _deferredResolver.CompleteDeferredCrypto(_testGroup, _workingResult, suppliedResult);
            
            if (!serverResult.Tag.Equals(suppliedResult.Tag))
            {
                errors.Add($"{nameof(suppliedResult.Tag)} does not match");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestCaseValidatorAftNoKdfNoKc : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _workingResult;
        private readonly TestGroup _testGroup;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, KasResult> _deferredResolver;

        private readonly SchemeKeyNonceGenRequirement<EccScheme> _iutKeyRequirements;

        public TestCaseValidatorAftNoKdfNoKc(TestCase workingResult, TestGroup testGroup, IDeferredTestCaseResolver<TestGroup, TestCase, KasResult> deferredResolver)
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

            if (suppliedResult.HashZ == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.HashZ)} but was not supplied");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            KasResult serverResult = _deferredResolver.CompleteDeferredCrypto(_testGroup, _workingResult, suppliedResult);

            if (!serverResult.Tag.Equals(suppliedResult.HashZ))
            {
                errors.Add($"{nameof(suppliedResult.HashZ)} does not match");
            }
        }
    }
}
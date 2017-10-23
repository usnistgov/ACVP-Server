using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseValidatorAftNoKdfNoKc : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _testGroup;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, KasResult> _deferredResolver;

        private (FfcScheme scheme, KeyAgreementRole thisPartyKasRole, bool generatesStaticKeyPair, bool
            generatesEphemeralKeyPair) _iutKeyRequirements;

        public TestCaseValidatorAftNoKdfNoKc(TestCase expectedResult, TestGroup testGroup, IDeferredTestCaseResolver<TestGroup, TestCase, KasResult> deferredResolver)
        {
            _expectedResult = expectedResult;
            _testGroup = testGroup;
            _deferredResolver = deferredResolver;

            _iutKeyRequirements =
                SpecificationMapping.GetKeyGenerationOptionsForSchemeAndRole(_testGroup.Scheme, _testGroup.KasRole);
        }

        public int TestCaseId => _expectedResult.TestCaseId;

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
            if (_iutKeyRequirements.generatesStaticKeyPair)
            {
                if (suppliedResult.StaticPublicKeyIut == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.StaticPublicKeyIut)} but was not supplied");
                }
            }

            if (_iutKeyRequirements.generatesEphemeralKeyPair)
            {
                if (suppliedResult.EphemeralPublicKeyIut == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.EphemeralPublicKeyIut)} but was not supplied");
                }
            }

            if (suppliedResult.HashZ == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.HashZ)} but was not supplied");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            KasResult serverResult = _deferredResolver.CompleteDeferredCrypto(_testGroup, _expectedResult, suppliedResult);

            if (!serverResult.Tag.Equals(suppliedResult.HashZ))
            {
                errors.Add($"{nameof(suppliedResult.HashZ)} does not match");
            }
        }
    }
}
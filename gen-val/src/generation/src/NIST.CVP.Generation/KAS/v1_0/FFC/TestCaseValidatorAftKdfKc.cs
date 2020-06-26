using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KAS.v1_0.FFC
{
    public class TestCaseValidatorAftKdfKc : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _workingTest;
        private readonly TestGroup _testGroup;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult> _deferredResolver;

        public TestCaseValidatorAftKdfKc(
            TestCase workingTest, 
            TestGroup testGroup, 
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult> deferredResolver
        )
        {
            _workingTest = workingTest;
            _testGroup = testGroup;
            _deferredResolver = deferredResolver;
        }

        public int TestCaseId => _workingTest.TestCaseId;

        public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                await CheckResults(suppliedResult, errors, expected, provided);
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
            if (_testGroup.KeyNonceGenRequirementsIut.GeneratesStaticKeyPair)
            {
                if (suppliedResult.StaticPublicKeyIut == null)
                {
                    errors.Add($"Expected {nameof(suppliedResult.StaticPublicKeyIut)} but was not supplied");
                }
            }

            if (_testGroup.KeyNonceGenRequirementsIut.GeneratesEphemeralKeyPair)
            {
                if (suppliedResult.EphemeralPublicKeyIut == null)
                {
                    errors.Add($"Expected {nameof(suppliedResult.EphemeralPublicKeyIut)} but was not supplied");
                }
            }

            if (_testGroup.KeyNonceGenRequirementsIut.GeneratesDkmNonce)
            {
                if (suppliedResult.DkmNonceIut == null || suppliedResult.DkmNonceIut.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.DkmNonceIut)} but was not supplied");
                }
            }

            if (_testGroup.KeyNonceGenRequirementsIut.GeneratesEphemeralNonce)
            {
                if (suppliedResult.EphemeralNonceIut == null || suppliedResult.EphemeralNonceIut.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.EphemeralNonceIut)} but was not supplied");
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

        private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            KasResult serverResult = await _deferredResolver.CompleteDeferredCryptoAsync(_testGroup, _workingTest, suppliedResult);

            if (!serverResult.Success)
            {
                errors.Add($"Failed completing deferred crypto. {serverResult.ErrorMessage}");
                return;
            }
            
            if (!serverResult.Tag.Equals(suppliedResult.Tag))
            {
                errors.Add($"{nameof(suppliedResult.Tag)} does not match");
                expected.Add(nameof(serverResult.Tag), serverResult.Tag.ToHex());
                provided.Add(nameof(suppliedResult.Tag), suppliedResult.Tag.ToHex());
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2
{
    public class TestCaseValidatorAft : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _workingTest;
        private readonly TestGroup _testGroup;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult> _deferredTestCaseResolverAsync;

        public TestCaseValidatorAft(
            TestCase workingTest,
            TestGroup testGroup,
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult> deferredTestCaseResolverAsync)
        {
            _workingTest = workingTest;
            _testGroup = testGroup;
            _deferredTestCaseResolverAsync = deferredTestCaseResolverAsync;
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
            // Always check for DKM
            if (suppliedResult.Dkm == null || suppliedResult.Dkm.BitLength == 0)
            {
                errors.Add($"Expected {nameof(suppliedResult.Dkm)} but was not supplied");
            }

            if (ShouldSupplyNonce())
            {
                if (suppliedResult.IutNonce == null || suppliedResult.IutNonce.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.IutNonce)} but was not supplied");
                }
            }

            if (ShouldSupplyValueC())
            {
                if (suppliedResult.IutC == null || suppliedResult.IutC.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.IutC)} but was not supplied");
                }
            }

            if (ShouldSupplyTag())
            {
                if (suppliedResult.Tag == null || suppliedResult.Tag.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.Tag)} but was not supplied");
                }
            }

            if (ShouldSupplyInitiatorNonce())
            {
                if (suppliedResult.KdfParameter?.AdditionalInitiatorNonce == null ||
                    suppliedResult.KdfParameter?.AdditionalInitiatorNonce.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.KdfParameter.AdditionalInitiatorNonce)} but was not supplied");
                }
            }

            if (ShouldSupplyResponderNonce())
            {
                if (suppliedResult.KdfParameter?.AdditionalResponderNonce == null ||
                    suppliedResult.KdfParameter?.AdditionalResponderNonce.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.KdfParameter.AdditionalResponderNonce)} but was not supplied");
                }
            }
        }

        /// <summary>
        /// IUT should supply a nonce when KAS1 and party V.
        /// </summary>
        /// <returns></returns>
        private bool ShouldSupplyNonce()
        {
            var map = new List<(IfcScheme scheme, KeyAgreementRole role, string testType)>()
            {
                (IfcScheme.Kas1_basic, KeyAgreementRole.ResponderPartyV, "aft"),
                (IfcScheme.Kas1_partyV_keyConfirmation, KeyAgreementRole.ResponderPartyV, "aft"),
            };

            return map.TryFirst(w =>
                w.scheme == _testGroup.Scheme &&
                w.role == _testGroup.KasRole &&
                w.testType.Equals(_testGroup.TestType, StringComparison.OrdinalIgnoreCase), out var result);
        }

        /// <summary>
        /// C value should be provided when IUT is kas1 party u, kas2, or kts party u.
        /// </summary>
        /// <returns></returns>
        private bool ShouldSupplyValueC()
        {
            var map = new List<(IfcScheme scheme, KeyAgreementRole role)>()
            {
                (IfcScheme.Kas1_basic, KeyAgreementRole.InitiatorPartyU),
                (IfcScheme.Kas1_partyV_keyConfirmation, KeyAgreementRole.InitiatorPartyU),
                (IfcScheme.Kas2_basic, KeyAgreementRole.InitiatorPartyU),
                (IfcScheme.Kas2_basic, KeyAgreementRole.ResponderPartyV),
                (IfcScheme.Kas2_bilateral_keyConfirmation, KeyAgreementRole.InitiatorPartyU),
                (IfcScheme.Kas2_bilateral_keyConfirmation, KeyAgreementRole.ResponderPartyV),
                (IfcScheme.Kas2_partyU_keyConfirmation, KeyAgreementRole.InitiatorPartyU),
                (IfcScheme.Kas2_partyU_keyConfirmation, KeyAgreementRole.ResponderPartyV),
                (IfcScheme.Kas2_partyV_keyConfirmation, KeyAgreementRole.InitiatorPartyU),
                (IfcScheme.Kas2_partyV_keyConfirmation, KeyAgreementRole.ResponderPartyV),
                (IfcScheme.Kts_oaep_basic, KeyAgreementRole.InitiatorPartyU),
                (IfcScheme.Kts_oaep_basic, KeyAgreementRole.InitiatorPartyU),
                (IfcScheme.Kts_oaep_partyV_keyConfirmation, KeyAgreementRole.InitiatorPartyU),
                (IfcScheme.Kts_oaep_partyV_keyConfirmation, KeyAgreementRole.InitiatorPartyU),
            };

            return map.TryFirst(w => w.scheme == _testGroup.Scheme && w.role == _testGroup.KasRole, out var result);
        }

        /// <summary>
        /// Tag should be supplied for all key confirmation schemes 
        /// </summary>
        /// <returns></returns>
        private bool ShouldSupplyTag()
        {
            return KeyGenerationRequirementsHelper.IfcKcSchemes.Contains(_testGroup.Scheme);
        }

        /// <summary>
        /// When an AFT test if the IUT is partyU for this group, then the additional nonce is required.
        /// </summary>
        /// <returns></returns>
        private bool ShouldSupplyInitiatorNonce()
        {
            if (_testGroup.TestType.Equals("VAL", StringComparison.OrdinalIgnoreCase) || _testGroup.KdfConfiguration == null)
            {
                return false;
            }

            return _testGroup.KdfConfiguration.RequiresAdditionalNoncePair && _testGroup.KasRole == KeyAgreementRole.InitiatorPartyU;
        }

        /// <summary>
        /// When an AFT test if the IUT is partyV for this group, then the additional nonce is required.
        /// </summary>
        /// <returns></returns>
        private bool ShouldSupplyResponderNonce()
        {
            if (_testGroup.TestType.Equals("VAL", StringComparison.OrdinalIgnoreCase) || _testGroup.KdfConfiguration == null)
            {
                return false;
            }

            return _testGroup.KdfConfiguration.RequiresAdditionalNoncePair && _testGroup.KasRole == KeyAgreementRole.ResponderPartyV;
        }

        private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            KasResult serverResult = await _deferredTestCaseResolverAsync.CompleteDeferredCryptoAsync(
                _testGroup, _workingTest, suppliedResult
            );

            if (!serverResult.Success)
            {
                errors.Add($"Failed completing deferred crypto. {serverResult.ErrorMessage}");
                return;
            }

            if (!serverResult.Dkm.Equals(suppliedResult.Dkm))
            {
                errors.Add($"{nameof(suppliedResult.Dkm)} does not match");
                expected.Add(nameof(serverResult.Dkm), serverResult.Dkm.ToHex());
                provided.Add(nameof(suppliedResult.Dkm), suppliedResult.Dkm.ToHex());
            }

            if (serverResult.Tag != null)
            {
                if (!serverResult.Tag.Equals(suppliedResult.Tag))
                {
                    errors.Add($"{nameof(suppliedResult.Tag)} does not match");
                    expected.Add(nameof(serverResult.Tag), serverResult.Tag.ToHex());
                    provided.Add(nameof(suppliedResult.Tag), suppliedResult.Tag.ToHex());
                }
            }
        }
    }
}

using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.KAS.FFC.Helpers;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public abstract class TestCaseGeneratorAftBaseFfc : TestCaseGeneratorAftBase<
        TestGroup,
        TestCase,
        KasDsaAlgoAttributesFfc,
        FfcDomainParameters,
        FfcKeyPair,
        FfcScheme
    >
    {

        protected TestCaseGeneratorAftBaseFfc(
            IKasBuilder<
                KasDsaAlgoAttributesFfc, 
                OtherPartySharedInformation<
                    FfcDomainParameters, 
                    FfcKeyPair
                >, 
                FfcDomainParameters, 
                FfcKeyPair
            > kasBuilder, 
            ISchemeBuilder<
                KasDsaAlgoAttributesFfc, 
                OtherPartySharedInformation<
                    FfcDomainParameters, 
                    FfcKeyPair
                >, 
                FfcDomainParameters, 
                FfcKeyPair
            > schemeBuilder, 
            IEntropyProviderFactory entropyProviderFactory, 
            IMacParametersBuilder macParametersBuilder
        ) : base(kasBuilder, schemeBuilder, entropyProviderFactory, macParametersBuilder) { }

        /// <inheritdoc />
        protected override SchemeKeyNonceGenRequirement<FfcScheme> GetPartyNonceKeyGenRequirements(
            TestGroup testGroup,
            KeyAgreementRole partyKeyAgreementRole, 
            KeyConfirmationRole partyKeyConfirmationRole
        )
        {
            return KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                testGroup.Scheme,
                testGroup.KasMode,
                partyKeyAgreementRole,
                partyKeyConfirmationRole,
                testGroup.KcType
            );
        }

        /// <inheritdoc />
        protected override int GetEphemeralLengthRequirement(TestGroup testGroup)
        {
            return ParameterSetDetails.GetDetailsForFfcParameterSet(testGroup.ParmSet).pLength;
        }

        /// <inheritdoc />
        protected override int GetDkmLengthRequirement(TestGroup testGroup)
        {
            return ParameterSetDetails.GetDetailsForFfcParameterSet(testGroup.ParmSet).qLength / 2;
        }

        /// <inheritdoc />
        protected override FfcDomainParameters GetGroupDomainParameters(TestGroup testGroup)
        {
            return new FfcDomainParameters(testGroup.P, testGroup.Q, testGroup.G);
        }

        /// <inheritdoc />
        protected override void SetTestCaseInformationFromKasResult(
            TestGroup @group, 
            TestCase testCase, 
            IKas<
                KasDsaAlgoAttributesFfc, 
                OtherPartySharedInformation<
                    FfcDomainParameters, 
                    FfcKeyPair
                >, 
                FfcDomainParameters, 
                FfcKeyPair
            > serverKas, 
            IKas<
                KasDsaAlgoAttributesFfc, 
                OtherPartySharedInformation<
                    FfcDomainParameters, 
                    FfcKeyPair
                >, 
                FfcDomainParameters, 
                FfcKeyPair
            > iutKas,
            KasResult iutResult)
        {
            TestCaseDispositionHelper.SetTestCaseInformationFromKasResults(
                group, 
                testCase, 
                serverKas, 
                iutKas,
                iutResult
            );
        }
    }
}
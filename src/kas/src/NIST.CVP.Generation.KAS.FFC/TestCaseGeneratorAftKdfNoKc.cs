using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorAftKdfNoKc : TestCaseGeneratorAftBaseFfc
    {
        
        public TestCaseGeneratorAftKdfNoKc(
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
        
        protected override IKas<
            KasDsaAlgoAttributesFfc, 
            OtherPartySharedInformation<
                FfcDomainParameters, 
                FfcKeyPair
            >, 
            FfcDomainParameters, 
            FfcKeyPair
        > GetKasInstance(
            SchemeKeyNonceGenRequirement<FfcScheme> partyKeyNonceRequirements, 
            KeyAgreementRole partyRole,
            KeyConfirmationRole partyKcRole, 
            MacParameters macParameters, 
            TestGroup @group, 
            TestCase testCase,
            BitString partyId
        )
        {
            return _kasBuilder
                .WithAssurances(KasAssurance.None)
                .WithKasDsaAlgoAttributes(group.KasDsaAlgoAttributes)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithHashFunction(group.HashAlg)
                )
                .WithPartyId(partyId)
                .WithKeyAgreementRole(partyRole)
                .BuildKdfNoKc()
                .WithKeyLength(group.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(group.OiPattern)
                .Build();
        }
    }
}
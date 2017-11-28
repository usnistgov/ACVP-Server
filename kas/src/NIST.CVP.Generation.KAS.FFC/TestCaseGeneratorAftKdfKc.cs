using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorAftKdfKc : TestCaseGeneratorAftBaseFfc
    {
        
        public TestCaseGeneratorAftKdfKc(
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
        ) : base (kasBuilder, schemeBuilder, entropyProviderFactory, macParametersBuilder) { }
        
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
                .BuildKdfKc()
                .WithKeyLength(group.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(group.OiPattern)
                .WithKeyConfirmationRole(partyKcRole)
                .WithKeyConfirmationDirection(group.KcType)
                .Build();
        }
    }
}
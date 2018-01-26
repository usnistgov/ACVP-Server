using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;

namespace NIST.CVP.Crypto.KAS.Builders.Ffc
{
    public class KasBuilderFfc 
        : KasBuilderBase<
            KasDsaAlgoAttributesFfc, 
            OtherPartySharedInformation<
                FfcDomainParameters, 
                FfcKeyPair
            >, 
            FfcDomainParameters, 
            FfcKeyPair
        >
    {
        public KasBuilderFfc(ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> schemeBuilder) : base(schemeBuilder)
        {
        }
        
        /// <inheritdoc />
        public override IKasBuilderNoKdfNoKc<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> BuildNoKdfNoKc()
        {
            return new KasBuilderNoKdfNoKcFfc(_schemeBuilder, _kasDsaAlgoAttributes, _keyAgreementRole, _assurances, _partyId);
        }

        /// <inheritdoc />
        public override IKasBuilderKdfNoKc<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> BuildKdfNoKc()
        {
            return new KasBuilderKdfNoKcFfc(_schemeBuilder, _kasDsaAlgoAttributes, _keyAgreementRole, _assurances, _partyId);
        }

        /// <inheritdoc />
        public override IKasBuilderKdfKc<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> BuildKdfKc()
        {
            return new KasBuilderKdfKcFfc(_schemeBuilder, _kasDsaAlgoAttributes, _keyAgreementRole, _assurances, _partyId);
        }
    }
}
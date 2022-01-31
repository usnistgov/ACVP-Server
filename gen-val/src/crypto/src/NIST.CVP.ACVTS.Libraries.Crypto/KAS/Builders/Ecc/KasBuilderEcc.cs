using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ecc;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ecc
{
    public class KasBuilderEcc
        : KasBuilderBase<
            KasDsaAlgoAttributesEcc,
            OtherPartySharedInformation<
                EccDomainParameters,
                EccKeyPair
            >,
            EccDomainParameters,
            EccKeyPair
        >
    {
        public KasBuilderEcc(ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> schemeBuilder) : base(schemeBuilder)
        {
        }

        /// <inheritdoc />
        public override IKasBuilderNoKdfNoKc<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> BuildNoKdfNoKc()
        {
            return new KasBuilderNoKdfNoKcEcc(_schemeBuilder, _kasDsaAlgoAttributes, _keyAgreementRole, _assurances, _partyId);
        }

        /// <inheritdoc />
        public override IKasBuilderKdfNoKc<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> BuildKdfNoKc()
        {
            return new KasBuilderKdfNoKcEcc(_schemeBuilder, _kasDsaAlgoAttributes, _keyAgreementRole, _assurances, _partyId);
        }

        /// <inheritdoc />
        public override IKasBuilderKdfKc<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> BuildKdfKc()
        {
            return new KasBuilderKdfKcEcc(_schemeBuilder, _kasDsaAlgoAttributes, _keyAgreementRole, _assurances, _partyId);
        }
    }
}

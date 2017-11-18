using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS.Builders.Ecc
{
    public class KasBuilderEcc
        : KasBuilderBase<
            EccParameterSet,
            EccScheme,
            OtherPartySharedInformation<
                EccDomainParameters,
                EccKeyPair
            >,
            EccDomainParameters,
            EccKeyPair
        >
    {
        public KasBuilderEcc(ISchemeBuilder<EccParameterSet, EccScheme, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> schemeBuilder) : base(schemeBuilder)
        {
        }

        /// <inheritdoc />
        public override IKasBuilderNoKdfNoKc<EccParameterSet, EccScheme, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> BuildNoKdfNoKc()
        {
            return new KasBuilderNoKdfNoKcEcc(_schemeBuilder, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId);
        }

        /// <inheritdoc />
        public override IKasBuilderKdfNoKc<EccParameterSet, EccScheme, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> BuildKdfNoKc()
        {
            return new KasBuilderKdfNoKcEcc(_schemeBuilder, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId);
        }

        /// <inheritdoc />
        public override IKasBuilderKdfKc<EccParameterSet, EccScheme, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> BuildKdfKc()
        {
            return new KasBuilderKdfKcEcc(_schemeBuilder, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId);
        }
    }
}
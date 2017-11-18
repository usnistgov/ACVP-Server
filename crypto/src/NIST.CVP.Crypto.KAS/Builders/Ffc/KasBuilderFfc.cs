using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS.Builders.Ffc
{
    public class KasBuilderFfc 
        : KasBuilderBase<
            FfcParameterSet, 
            FfcScheme, 
            OtherPartySharedInformation<
                FfcDomainParameters, 
                FfcKeyPair
            >, 
            FfcDomainParameters, 
            FfcKeyPair
        >
    {
        public KasBuilderFfc(ISchemeBuilder<FfcParameterSet, FfcScheme, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> schemeBuilder) : base(schemeBuilder)
        {
        }
        
        /// <inheritdoc />
        public override IKasBuilderNoKdfNoKc<FfcParameterSet, FfcScheme, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> BuildNoKdfNoKc()
        {
            return new KasBuilderNoKdfNoKcFfc(_schemeBuilder, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId);
        }

        /// <inheritdoc />
        public override IKasBuilderKdfNoKc<FfcParameterSet, FfcScheme, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> BuildKdfNoKc()
        {
            return new KasBuilderKdfNoKcFfc(_schemeBuilder, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId);
        }

        /// <inheritdoc />
        public override IKasBuilderKdfKc<FfcParameterSet, FfcScheme, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> BuildKdfKc()
        {
            return new KasBuilderKdfKcFfc(_schemeBuilder, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId);
        }
    }
}
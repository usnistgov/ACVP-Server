using NIST.CVP.Crypto.KAS.Enums;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public class KasBuilderFfc : KasBuilderBase<FfcParameterSet, FfcScheme>
    {
        public KasBuilderFfc(ISchemeBuilder<FfcParameterSet, FfcScheme> schemeBuilder) : base(schemeBuilder)
        {
        }
        
        /// <inheritdoc />
        public override IKasBuilderNoKdfNoKc<FfcParameterSet, FfcScheme> BuildNoKdfNoKc()
        {
            return new KasBuilderNoKdfNoKcFfc(_schemeBuilder, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId);
        }

        /// <inheritdoc />
        public override IKasBuilderKdfNoKc<FfcParameterSet, FfcScheme> BuildKdfNoKc()
        {
            return new KasBuilderKdfNoKcFfc(_schemeBuilder, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId);
        }

        /// <inheritdoc />
        public override IKasBuilderKdfKc<FfcParameterSet, FfcScheme> BuildKdfKc()
        {
            return new KasBuilderKdfKcFfc(_schemeBuilder, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId);
        }
    }
}
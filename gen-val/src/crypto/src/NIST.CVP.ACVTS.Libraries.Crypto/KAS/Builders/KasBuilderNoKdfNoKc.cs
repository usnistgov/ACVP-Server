using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders
{
    public abstract class KasBuilderNoKdfNoKc<TKasDsaAlgoAttributes, TSharedInformation, TDomainParameters, TKeyPair>
        : IKasBuilderNoKdfNoKc<TKasDsaAlgoAttributes, TSharedInformation, TDomainParameters, TKeyPair>
        where TKasDsaAlgoAttributes : IKasAlgoAttributes
        where TSharedInformation : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        protected readonly ISchemeBuilder<TKasDsaAlgoAttributes, TSharedInformation, TDomainParameters, TKeyPair> _schemeBuilder;
        protected readonly TKasDsaAlgoAttributes _kasDsaAlgoAttributes;
        protected readonly KeyAgreementRole _keyAgreementRole;
        protected readonly KasAssurance _assurances;
        protected readonly BitString _partyId;

        protected KasBuilderNoKdfNoKc(
            ISchemeBuilder<TKasDsaAlgoAttributes, TSharedInformation, TDomainParameters, TKeyPair> schemeBuilder,
            TKasDsaAlgoAttributes kasDsaAlgoAttributes,
            KeyAgreementRole keyAgreementRole,
            KasAssurance assurances,
            BitString partyId
        )
        {
            _schemeBuilder = schemeBuilder;
            _kasDsaAlgoAttributes = kasDsaAlgoAttributes;
            _keyAgreementRole = keyAgreementRole;
            _assurances = assurances;
            _partyId = partyId;
        }

        public abstract IKas<TKasDsaAlgoAttributes, TSharedInformation, TDomainParameters, TKeyPair> Build();
    }
}

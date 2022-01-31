using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders
{
    public abstract class KasBuilderBase<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        : IKasBuilder<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TKasDsaAlgoAttributes : IKasAlgoAttributes
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        protected KeyAgreementRole _keyAgreementRole;
        protected TKasDsaAlgoAttributes _kasDsaAlgoAttributes;
        protected KasAssurance _assurances;
        protected BitString _partyId;
        protected ISchemeBuilder<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> _schemeBuilder;

        protected KasBuilderBase(ISchemeBuilder<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> schemeBuilder)
        {
            _schemeBuilder = schemeBuilder;
        }

        /// <inheritdoc />
        public IKasBuilder<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        >
            WithKeyAgreementRole(KeyAgreementRole value)
        {
            _keyAgreementRole = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        >
            WithKasDsaAlgoAttributes(TKasDsaAlgoAttributes value)
        {
            _kasDsaAlgoAttributes = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        >
            WithSchemeBuilder(
                ISchemeBuilder<
                    TKasDsaAlgoAttributes,
                    TOtherPartySharedInfo,
                    TDomainParameters,
                    TKeyPair
                > schemeBuilder
            )
        {
            _schemeBuilder = schemeBuilder;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        >
            WithAssurances(KasAssurance value)
        {
            _assurances = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        >
            WithPartyId(BitString value)
        {
            _partyId = value;
            return this;
        }

        /// <inheritdoc />
        public abstract IKasBuilderKdfKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        >
            BuildKdfKc();

        /// <inheritdoc />
        public abstract IKasBuilderKdfNoKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        >
            BuildKdfNoKc();

        /// <inheritdoc />
        public abstract IKasBuilderNoKdfNoKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        >
            BuildNoKdfNoKc();

    }
}

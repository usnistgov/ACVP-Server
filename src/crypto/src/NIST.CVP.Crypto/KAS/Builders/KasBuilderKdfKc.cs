using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public abstract class KasBuilderKdfKc<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> 
        : IKasBuilderKdfKc<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TKasDsaAlgoAttributes : IKasAlgoAttributes
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        protected readonly ISchemeBuilder<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> _schemeBuilder;
        protected readonly TKasDsaAlgoAttributes _kasDsaAlgoAttributes;
        protected readonly KeyAgreementRole _keyAgreementRole;
        protected readonly KasAssurance _assurances;
        protected readonly BitString _partyId;
        protected int _keyLength;
        protected string _otherInfoPattern = OtherInfo._CAVS_OTHER_INFO_PATTERN;
        protected MacParameters _macParameters;
        protected KeyConfirmationRole _keyConfirmationRole;
        protected KeyConfirmationDirection _keyConfirmationDirection;
       

        protected KasBuilderKdfKc(
            ISchemeBuilder<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> schemeBuilder,
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

        /// <summary>
        /// Sets the keyLength for the <see cref="IKdfOneStep"/> options in the <see cref="IKas{TKasDsaAlgoAttributes,TOtherPartySharedInfo,TDomainParameters,TKeyPair}"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IKasBuilderKdfKc<
            TKasDsaAlgoAttributes, 
            TOtherPartySharedInfo, 
            TDomainParameters, 
            TKeyPair
        > WithKeyLength(int value)
        {
            _keyLength = value;
            return this;
        }

        /// <summary>
        /// Sets the otherInfoPattern for the <see cref="IKdfOneStep"/> options in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IKasBuilderKdfKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > WithOtherInfoPattern(string value)
        {
            _otherInfoPattern = value;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="MacParameters"/> in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IKasBuilderKdfKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > WithMacParameters(MacParameters value)
        {
            _macParameters = value;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="KeyConfirmationRole"/> for the <see cref="IKeyConfirmation"/> in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IKasBuilderKdfKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > WithKeyConfirmationRole(KeyConfirmationRole value)
        {
            _keyConfirmationRole = value;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="KeyConfirmationDirection"/> for the <see cref="IKeyConfirmation"/> in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IKasBuilderKdfKc<
            TKasDsaAlgoAttributes,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > WithKeyConfirmationDirection(KeyConfirmationDirection value)
        {
            _keyConfirmationDirection = value;
            return this;
        }

        public abstract IKas<TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Build();
    }
}
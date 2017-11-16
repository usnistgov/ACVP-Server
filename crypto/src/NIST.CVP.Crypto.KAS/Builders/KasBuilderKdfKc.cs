using System;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public abstract class KasBuilderKdfKc<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> 
        : IKasBuilderKdfKc<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        protected readonly ISchemeBuilder<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> _schemeBuilder;
        protected readonly KeyAgreementRole _keyAgreementRole;
        protected readonly TScheme _scheme;
        protected readonly TParameterSet _parameterSet;
        protected readonly KasAssurance _assurances;
        protected readonly BitString _partyId;
        protected int _keyLength;
        protected string _otherInfoPattern = OtherInfo<TOtherPartySharedInfo, TDomainParameters, TKeyPair>._CAVS_OTHER_INFO_PATTERN;
        protected MacParameters _macParameters;
        protected KeyConfirmationRole _keyConfirmationRole;
        protected KeyConfirmationDirection _keyConfirmationDirection;
       

        protected KasBuilderKdfKc(
            ISchemeBuilder<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> schemeBuilder, 
            KeyAgreementRole keyAgreementRole, 
            TScheme scheme, 
            TParameterSet parameterSet, 
            KasAssurance assurances, 
            BitString partyId
         )
        {
            _schemeBuilder = schemeBuilder;
            _keyAgreementRole = keyAgreementRole;
            _scheme = scheme;
            _parameterSet = parameterSet;
            _assurances = assurances;
            _partyId = partyId;
        }

        /// <summary>
        /// Sets the keyLength for the <see cref="IKdf"/> options in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IKasBuilderKdfKc<
            TParameterSet, 
            TScheme, 
            TOtherPartySharedInfo, 
            TDomainParameters, 
            TKeyPair
        > WithKeyLength(int value)
        {
            _keyLength = value;
            return this;
        }

        /// <summary>
        /// Sets the otherInfoPattern for the <see cref="IKdf"/> options in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IKasBuilderKdfKc<
            TParameterSet,
            TScheme,
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
            TParameterSet,
            TScheme,
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
            TParameterSet,
            TScheme,
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
            TParameterSet,
            TScheme,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > WithKeyConfirmationDirection(KeyConfirmationDirection value)
        {
            _keyConfirmationDirection = value;
            return this;
        }

        public abstract IKas<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> Build();
    }
}
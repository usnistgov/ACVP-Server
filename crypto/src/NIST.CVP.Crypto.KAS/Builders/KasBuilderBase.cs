using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public abstract class KasBuilderBase<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> : IKasBuilder<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        protected KeyAgreementRole _keyAgreementRole;
        protected TParameterSet _parameterSet;
        protected TScheme _scheme;
        protected KasAssurance _assurances;
        protected BitString _partyId;
        protected ISchemeBuilder<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> _schemeBuilder;

        protected KasBuilderBase(ISchemeBuilder<TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> schemeBuilder)
        {
            _schemeBuilder = schemeBuilder;
        }

        /// <inheritdoc />
        public IKasBuilder<
            TParameterSet, 
            TScheme, 
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
            TParameterSet,
            TScheme,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithScheme(TScheme value)
        {
            _scheme = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder<
            TParameterSet,
            TScheme,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithParameterSet(TParameterSet value)
        {
            _parameterSet = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder<
            TParameterSet,
            TScheme,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            WithSchemeBuilder(
                ISchemeBuilder<
                    TParameterSet, 
                    TScheme, 
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
            TParameterSet,
            TScheme,
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
            TParameterSet,
            TScheme,
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
            TParameterSet, 
            TScheme, 
            TOtherPartySharedInfo, 
            TDomainParameters, 
            TKeyPair
        > 
            BuildKdfKc();

        /// <inheritdoc />
        public abstract IKasBuilderKdfNoKc<
            TParameterSet,
            TScheme,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            BuildKdfNoKc();

        /// <inheritdoc />
        public abstract IKasBuilderNoKdfNoKc<
            TParameterSet,
            TScheme,
            TOtherPartySharedInfo,
            TDomainParameters,
            TKeyPair
        > 
            BuildNoKdfNoKc();

    }
}

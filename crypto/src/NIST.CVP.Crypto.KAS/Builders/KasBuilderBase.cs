using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public abstract class KasBuilderBase<TParameterSet, TScheme> : IKasBuilder<TParameterSet, TScheme>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
    {
        protected KeyAgreementRole _keyAgreementRole;
        protected TParameterSet _parameterSet;
        protected TScheme _scheme;
        protected KasAssurance _assurances;
        protected BitString _partyId;
        protected ISchemeBuilder<TParameterSet, TScheme> _schemeBuilder;

        protected KasBuilderBase(ISchemeBuilder<TParameterSet, TScheme> schemeBuilder)
        {
            _schemeBuilder = schemeBuilder;
        }

        /// <inheritdoc />
        public IKasBuilder<TParameterSet, TScheme> WithKeyAgreementRole(KeyAgreementRole value)
        {
            _keyAgreementRole = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder<TParameterSet, TScheme> WithScheme(TScheme value)
        {
            _scheme = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder<TParameterSet, TScheme> WithParameterSet(TParameterSet value)
        {
            _parameterSet = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder<TParameterSet, TScheme> WithSchemeBuilder(ISchemeBuilder<TParameterSet, TScheme> schemeBuilder)
        {
            _schemeBuilder = schemeBuilder;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder<TParameterSet, TScheme> WithAssurances(KasAssurance value)
        {
            _assurances = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder<TParameterSet, TScheme> WithPartyId(BitString value)
        {
            _partyId = value;
            return this;
        }

        /// <inheritdoc />
        public abstract IKasBuilderKdfKc<TParameterSet, TScheme> BuildKdfKc();

        /// <inheritdoc />
        public abstract IKasBuilderKdfNoKc<TParameterSet, TScheme> BuildKdfNoKc();

        /// <inheritdoc />
        public abstract IKasBuilderNoKdfNoKc<TParameterSet, TScheme> BuildNoKdfNoKc();

    }
}

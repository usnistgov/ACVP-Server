using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public class KasBuilder : IKasBuilder
    {
        private KeyAgreementRole _keyAgreementRole;
        private FfcParameterSet _parameterSet;
        private FfcScheme _scheme;
        private KasAssurance _assurances;
        private BitString _partyId;
        private ISchemeBuilder _schemeBuilder;

        /// <inheritdoc />
        public IKasBuilder WithKeyAgreementRole(KeyAgreementRole value)
        {
            _keyAgreementRole = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder WithScheme(FfcScheme value)
        {
            _scheme = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder WithParameterSet(FfcParameterSet value)
        {
            _parameterSet = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder WithSchemeBuilder(ISchemeBuilder schemeBuilder)
        {
            _schemeBuilder = schemeBuilder;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder WithAssurances(KasAssurance value)
        {
            _assurances = value;
            return this;
        }

        /// <inheritdoc />
        public IKasBuilder WithPartyId(BitString value)
        {
            _partyId = value;
            return this;
        }

        /// <inheritdoc />
        public KasBuilderNoKdfNoKc BuildNoKdfNoKc()
        {
            return new KasBuilderNoKdfNoKc(_schemeBuilder, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId);
        }

        /// <inheritdoc />
        public KasBuilderKdfNoKc BuildKdfNoKc()
        {
            return new KasBuilderKdfNoKc(_schemeBuilder, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId);
        }

        /// <inheritdoc />
        public KasBuilderKdfKc BuildKdfKc()
        {
            return new KasBuilderKdfKc(_schemeBuilder, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId);
        }
    }
}

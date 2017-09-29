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
        private readonly ISchemeFactory _schemeFactory;
        private KeyAgreementRole _keyAgreementRole;
        private FfcParameterSet _parameterSet;
        private FfcScheme _scheme;
        private KasAssurance _assurances;
        private BitString _partyId;

        public KasBuilder(ISchemeFactory schemeFactory)
        {
            _schemeFactory = schemeFactory;
        }

        /// <inheritdoc />
        public KasBuilder WithKeyAgreementRole(KeyAgreementRole value)
        {
            _keyAgreementRole = value;
            return this;
        }

        /// <inheritdoc />
        public KasBuilder WithScheme(FfcScheme value)
        {
            _scheme = value;
            return this;
        }

        /// <inheritdoc />
        public KasBuilder WithParameterSet(FfcParameterSet value)
        {
            _parameterSet = value;
            return this;
        }

        /// <inheritdoc />
        public KasBuilder WithAssurances(KasAssurance value)
        {
            _assurances = value;
            return this;
        }

        /// <inheritdoc />
        public KasBuilder WithPartyId(BitString value)
        {
            _partyId = value;
            return this;
        }

        /// <inheritdoc />
        public KasBuilderNoKdfNoKc BuildNoKdfNoKc()
        {
            return new KasBuilderNoKdfNoKc(_schemeFactory, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId, KasMode.NoKdfNoKc);
        }

        /// <inheritdoc />
        public KasBuilderKdfNoKc BuildKdfNoKc()
        {
            return new KasBuilderKdfNoKc(_schemeFactory, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId, KasMode.KdfNoKc);
        }

        /// <inheritdoc />
        public KasBuilderKdfKc BuildKdfKc()
        {
            return new KasBuilderKdfKc(_schemeFactory, _keyAgreementRole, _scheme, _parameterSet, _assurances, _partyId, KasMode.KdfKc);
        }
    }
}

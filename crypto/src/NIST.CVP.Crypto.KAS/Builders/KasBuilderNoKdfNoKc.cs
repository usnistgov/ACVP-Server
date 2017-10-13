using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public class KasBuilderNoKdfNoKc : IKasBuilderNoKdfNoKc
    {
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly KeyAgreementRole _keyAgreementRole;
        private readonly FfcScheme _scheme;
        private readonly FfcParameterSet _parameterSet;
        private readonly KasAssurance _assurances;
        private readonly BitString _partyId;

        public KasBuilderNoKdfNoKc(ISchemeBuilder schemeBuilder, KeyAgreementRole keyAgreementRole, FfcScheme scheme, FfcParameterSet parameterSet, KasAssurance assurances, BitString partyId)
        {
            _schemeBuilder = schemeBuilder;
            _keyAgreementRole = keyAgreementRole;
            _scheme = scheme;
            _parameterSet = parameterSet;
            _assurances = assurances;
            _partyId = partyId;
        }

        /// <summary>
        /// Builds and returns the <see cref="IKas"/>
        /// </summary>
        /// <returns></returns>
        public IKas Build()
        {
            var schemeParameters = new SchemeParameters(
                _keyAgreementRole,
                KasMode.NoKdfNoKc,
                _scheme,
                KeyConfirmationRole.None,
                KeyConfirmationDirection.None,
                _parameterSet,
                _assurances,
                _partyId
            );
            var scheme = _schemeBuilder.BuildScheme(schemeParameters, null, null);

            return new Kas(scheme);
        }
    }
}
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public class KasBuilderNoKdfNoKc
    {
        private readonly ISchemeFactory _schemeFactory;
        private readonly KeyAgreementRole _keyAgreementRole;
        private readonly FfcScheme _scheme;
        private readonly FfcParameterSet _parameterSet;
        private readonly KasAssurance _assurances;
        private readonly BitString _partyId;
        private readonly KasMode _kasMode;

        public KasBuilderNoKdfNoKc(ISchemeFactory schemeFactory, KeyAgreementRole keyAgreementRole, FfcScheme scheme, FfcParameterSet parameterSet, KasAssurance assurances, BitString partyId, KasMode kasMode)
        {
            _schemeFactory = schemeFactory;
            _keyAgreementRole = keyAgreementRole;
            _scheme = scheme;
            _parameterSet = parameterSet;
            _assurances = assurances;
            _partyId = partyId;
            _kasMode = kasMode;
        }

        /// <summary>
        /// Builds and returns the <see cref="IKas"/>
        /// </summary>
        /// <returns></returns>
        public IKas Build()
        {
            var schemeParameters = new SchemeParameters(
                _keyAgreementRole,
                KasMode.NoKdf,
                _scheme,
                KeyConfirmationRole.None,
                KeyConfirmationDirection.None,
                _parameterSet,
                _assurances,
                _partyId
            );
            var scheme = _schemeFactory.GetInstance(schemeParameters, null, null);

            return new Kas(scheme);
        }
    }
}
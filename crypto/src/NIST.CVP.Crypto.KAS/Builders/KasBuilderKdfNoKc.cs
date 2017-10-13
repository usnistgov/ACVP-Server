using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public class KasBuilderKdfNoKc : IKasBuilderKdfNoKc
    {
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly KeyAgreementRole _keyAgreementRole;
        private readonly FfcScheme _scheme;
        private readonly FfcParameterSet _parameterSet;
        private readonly KasAssurance _assurances;
        private readonly BitString _partyId;
        private int _keyLength;
        private string _otherInfoPattern = OtherInfo._CAVS_OTHER_INFO_PATTERN;
        private MacParameters _macParameters;
        
        public KasBuilderKdfNoKc(ISchemeBuilder schemeBuilder, KeyAgreementRole keyAgreementRole, FfcScheme scheme, FfcParameterSet parameterSet, KasAssurance assurances, BitString partyId)
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
        public IKasBuilderKdfNoKc WithKeyLength(int value)
        {
            _keyLength = value;
            return this;
        }

        /// <summary>
        /// Sets the otherInfoPattern for the <see cref="IKdf"/> options in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IKasBuilderKdfNoKc WithOtherInfoPattern(string value)
        {
            _otherInfoPattern = value;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="MacParameters"/> in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IKasBuilderKdfNoKc WithMacParameters(MacParameters value)
        {
            _macParameters = value;
            return this;
        }

        /// <summary>
        /// Builds and returns the <see cref="IKas"/>
        /// </summary>
        /// <returns></returns>
        public IKas Build()
        {
            var schemeParameters = new SchemeParameters(
                _keyAgreementRole,
                KasMode.KdfNoKc,
                _scheme,
                KeyConfirmationRole.None,
                KeyConfirmationDirection.None,
                _parameterSet,
                _assurances,
                _partyId
            );

            var kdfParameters = new KdfParameters(_keyLength, _otherInfoPattern);
            var scheme = _schemeBuilder.BuildScheme(schemeParameters, kdfParameters, _macParameters);

            return new Kas(scheme);
        }
    }
}
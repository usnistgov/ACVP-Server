using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public class KasBuilderKdfKc
    {
        private readonly ISchemeFactory _schemeFactory;
        private readonly KeyAgreementRole _keyAgreementRole;
        private readonly FfcScheme _scheme;
        private readonly FfcParameterSet _parameterSet;
        private readonly KasAssurance _assurances;
        private readonly BitString _partyId;
        private readonly KasMode _kasMode;
        private int _keyLength;
        private string _otherInfoPattern = OtherInfo._CAVS_OTHER_INFO_PATTERN;
        private MacParameters _macParameters;
        private KeyConfirmationRole _keyConfirmationRole;
        private KeyConfirmationDirection _keyConfirmationDirection;
       

        public KasBuilderKdfKc(ISchemeFactory schemeFactory, KeyAgreementRole keyAgreementRole, FfcScheme scheme, FfcParameterSet parameterSet, KasAssurance assurances, BitString partyId, KasMode kasMode)
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
        /// Sets the keyLength for the <see cref="IKdf"/> options in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public KasBuilderKdfKc WithKeyLength(int value)
        {
            _keyLength = value;
            return this;
        }

        /// <summary>
        /// Sets the otherInfoPattern for the <see cref="IKdf"/> options in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public KasBuilderKdfKc WithOtherInfoPattern(string value)
        {
            _otherInfoPattern = value;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="MacParameters"/> in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public KasBuilderKdfKc WithMacParameters(MacParameters value)
        {
            _macParameters = value;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="KeyConfirmationRole"/> for the <see cref="IKeyConfirmation"/> in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public KasBuilderKdfKc WithKeyConfirmationRole(KeyConfirmationRole value)
        {
            _keyConfirmationRole = value;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="KeyConfirmationDirection"/> for the <see cref="IKeyConfirmation"/> in the <see cref="IKas"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public KasBuilderKdfKc WithKeyConfirmationDirection(KeyConfirmationDirection value)
        {
            _keyConfirmationDirection = value;
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
                KasMode.NoKdf,
                _scheme,
                _keyConfirmationRole,
                _keyConfirmationDirection,
                _parameterSet,
                _assurances,
                _partyId
            );

            var kdfParameters = new KdfParameters(_keyLength, _otherInfoPattern);
            var scheme = _schemeFactory.GetInstance(schemeParameters, kdfParameters, _macParameters);

            return new Kas(scheme);
        }
    }
}
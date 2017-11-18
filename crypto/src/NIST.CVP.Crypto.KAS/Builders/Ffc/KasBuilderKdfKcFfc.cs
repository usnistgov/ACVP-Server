using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders.Ffc
{
    public class KasBuilderKdfKcFfc 
        : KasBuilderKdfKc<
            FfcParameterSet, 
            FfcScheme, 
            OtherPartySharedInformation<
                FfcDomainParameters, 
                FfcKeyPair
            >, 
            FfcDomainParameters, 
            FfcKeyPair
        >
    {
        public KasBuilderKdfKcFfc(
            ISchemeBuilder<
                FfcParameterSet, 
                FfcScheme,
                OtherPartySharedInformation<
                    FfcDomainParameters,
                    FfcKeyPair
                >,
                FfcDomainParameters,
                FfcKeyPair
            > schemeBuilder, 
            KeyAgreementRole keyAgreementRole, 
            FfcScheme scheme, 
            FfcParameterSet parameterSet, 
            KasAssurance assurances, 
            BitString partyId) 
            : base(
                  schemeBuilder, 
                  keyAgreementRole, 
                  scheme, 
                  parameterSet, 
                  assurances, 
                  partyId
              )
        {
        }

        public override IKas<
            FfcParameterSet, 
            FfcScheme,
            OtherPartySharedInformation<
                FfcDomainParameters,
                FfcKeyPair
            >,
            FfcDomainParameters,
            FfcKeyPair
        > Build()
        {
            var schemeParameters = new SchemeParametersFfc(
                _keyAgreementRole,
                KasMode.KdfKc,
                _scheme,
                _keyConfirmationRole,
                _keyConfirmationDirection,
                _parameterSet,
                _assurances,
                _partyId
            );

            var kdfParameters = new KdfParameters(_keyLength, _otherInfoPattern);
            var scheme = _schemeBuilder.BuildScheme(schemeParameters, kdfParameters, _macParameters);

            return new Kas<
                FfcParameterSet, 
                FfcScheme,
                OtherPartySharedInformation<
                    FfcDomainParameters,
                    FfcKeyPair
                >,
                FfcDomainParameters,
                FfcKeyPair
            >(scheme);
        }
    }
}
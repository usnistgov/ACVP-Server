using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders.Ffc
{
    public class KasBuilderKdfNoKcFfc : 
        KasBuilderKdfNoKc<
            KasDsaAlgoAttributesFfc, 
            OtherPartySharedInformation<
                FfcDomainParameters, 
                FfcKeyPair
            >, 
            FfcDomainParameters, 
            FfcKeyPair
        >
    {
        public KasBuilderKdfNoKcFfc(
            ISchemeBuilder<
                KasDsaAlgoAttributesFfc, 
                OtherPartySharedInformation<
                    FfcDomainParameters, 
                    FfcKeyPair
                >, 
                FfcDomainParameters, 
                FfcKeyPair
            > schemeBuilder,
            KasDsaAlgoAttributesFfc kasDsaAlgoAttributes,
            KeyAgreementRole keyAgreementRole, 
            KasAssurance assurances, 
            BitString partyId) 
            : base(
                  schemeBuilder, 
                  kasDsaAlgoAttributes,
                  keyAgreementRole, 
                  assurances, 
                  partyId
              )
        {
        }

        public override IKas<
            KasDsaAlgoAttributesFfc, 
            OtherPartySharedInformation<
                FfcDomainParameters, 
                FfcKeyPair
            >, 
            FfcDomainParameters, 
            FfcKeyPair
        > Build()
        {
            var schemeParameters = new SchemeParametersFfc(
                _kasDsaAlgoAttributes,
                _keyAgreementRole,
                KasMode.KdfNoKc,
                KeyConfirmationRole.None,
                KeyConfirmationDirection.None,
                _assurances,
                _partyId
            );

            var kdfParameters = new KdfParameters(_keyLength, _otherInfoPattern);
            var scheme = _schemeBuilder.BuildScheme(schemeParameters, kdfParameters, _macParameters);

            return new Kas<
                KasDsaAlgoAttributesFfc, 
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
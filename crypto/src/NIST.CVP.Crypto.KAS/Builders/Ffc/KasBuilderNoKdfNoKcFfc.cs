using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders.Ffc
{
    public class KasBuilderNoKdfNoKcFfc 
        : KasBuilderNoKdfNoKc<
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
        public KasBuilderNoKdfNoKcFfc(
            ISchemeBuilder<FfcParameterSet, FfcScheme, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> schemeBuilder, 
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
                KasMode.NoKdfNoKc,
                _scheme,
                KeyConfirmationRole.None,
                KeyConfirmationDirection.None,
                _parameterSet,
                _assurances,
                _partyId
            );
            var scheme = _schemeBuilder.BuildScheme(schemeParameters, null, null);

            return new Kas<
                FfcParameterSet, 
                FfcScheme, 
                OtherPartySharedInformation<
                    FfcDomainParameters, 
                    FfcKeyPair
                >, 
                FfcDomainParameters, 
                FfcKeyPair
            > (scheme);
        }
    }
}
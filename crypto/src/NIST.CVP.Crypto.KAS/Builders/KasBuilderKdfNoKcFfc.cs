using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public class KasBuilderKdfNoKcFfc : KasBuilderKdfNoKc<FfcParameterSet, FfcScheme>
    {
        public KasBuilderKdfNoKcFfc(
            ISchemeBuilder<FfcParameterSet, FfcScheme> schemeBuilder, 
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

        public override IKas<FfcParameterSet, FfcScheme> Build()
        {
            var schemeParameters = new SchemeParametersFfc(
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

            return new Kas<FfcParameterSet, FfcScheme>(scheme);
        }
    }
}
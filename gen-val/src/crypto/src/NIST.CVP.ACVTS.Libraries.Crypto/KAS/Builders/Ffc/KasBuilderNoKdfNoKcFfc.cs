using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ffc
{
    public class KasBuilderNoKdfNoKcFfc
        : KasBuilderNoKdfNoKc<
            KasDsaAlgoAttributesFfc,
            OtherPartySharedInformation<
                FfcDomainParameters,
                FfcKeyPair
            >,
            FfcDomainParameters,
            FfcKeyPair
        >
    {
        public KasBuilderNoKdfNoKcFfc(
            ISchemeBuilder<
                KasDsaAlgoAttributesFfc,
                OtherPartySharedInformation<
                    FfcDomainParameters,
                    FfcKeyPair
                >,
                FfcDomainParameters,
                FfcKeyPair
            > schemeBuilder,
            KasDsaAlgoAttributesFfc kasDsaAlgoAttributesFfc,
            KeyAgreementRole keyAgreementRole,
            KasAssurance assurances,
            BitString partyId)
            : base(
                  schemeBuilder,
                  kasDsaAlgoAttributesFfc,
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
                KasMode.NoKdfNoKc,
                KeyConfirmationRole.None,
                KeyConfirmationDirection.None,
                _assurances,
                _partyId
            );
            var scheme = _schemeBuilder.BuildScheme(schemeParameters, null, null);

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

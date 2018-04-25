using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders.Ecc
{
    public class KasBuilderNoKdfNoKcEcc
            : KasBuilderNoKdfNoKc<
                KasDsaAlgoAttributesEcc,
                OtherPartySharedInformation<
                    EccDomainParameters,
                    EccKeyPair
                >,
                EccDomainParameters,
                EccKeyPair
            >
    {
        public KasBuilderNoKdfNoKcEcc(
            ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> schemeBuilder,
            KasDsaAlgoAttributesEcc kasDsaAlgoAttributes,
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
            KasDsaAlgoAttributesEcc,
            OtherPartySharedInformation<
                EccDomainParameters,
                EccKeyPair
            >,
            EccDomainParameters,
            EccKeyPair
        > Build()
        {
            var schemeParameters = new SchemeParametersEcc(
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
                KasDsaAlgoAttributesEcc,
                OtherPartySharedInformation<
                    EccDomainParameters,
                    EccKeyPair
                >,
                EccDomainParameters,
                EccKeyPair
            >(scheme);
        }
    }
}
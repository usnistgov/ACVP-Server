using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders.Ecc
{
    public class KasBuilderNoKdfNoKcEcc
            : KasBuilderNoKdfNoKc<
                EccParameterSet,
                EccScheme,
                OtherPartySharedInformation<
                    EccDomainParameters,
                    EccKeyPair
                >,
                EccDomainParameters,
                EccKeyPair
            >
    {
        public KasBuilderNoKdfNoKcEcc(
            ISchemeBuilder<EccParameterSet, EccScheme, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> schemeBuilder,
            KeyAgreementRole keyAgreementRole,
            EccScheme scheme,
            EccParameterSet parameterSet,
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
            EccParameterSet,
            EccScheme,
            OtherPartySharedInformation<
                EccDomainParameters,
                EccKeyPair
            >,
            EccDomainParameters,
            EccKeyPair
        > Build()
        {
            var schemeParameters = new SchemeParametersEcc(
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
                EccParameterSet,
                EccScheme,
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
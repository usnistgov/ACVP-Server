using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders.Ecc
{
    public class KasBuilderKdfNoKcEcc :
        KasBuilderKdfNoKc<
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
        public KasBuilderKdfNoKcEcc(
            ISchemeBuilder<
                EccParameterSet,
                EccScheme,
                OtherPartySharedInformation<
                    EccDomainParameters,
                    EccKeyPair
                >,
                EccDomainParameters,
                EccKeyPair
            > schemeBuilder,
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
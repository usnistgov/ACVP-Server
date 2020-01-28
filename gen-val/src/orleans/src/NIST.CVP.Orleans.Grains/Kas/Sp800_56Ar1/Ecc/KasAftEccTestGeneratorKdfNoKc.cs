using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar1.Ecc
{
    internal class KasAftEccTestGeneratorKdfNoKc : KasAftEccTestGeneratorBase
    {
        public KasAftEccTestGeneratorKdfNoKc(
            IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > schemeBuilder,
            IEntropyProviderFactory entropyProviderFactory,
            IMacParametersBuilder macParametersBuilder,
            IEccCurveFactory curveFactory
        ) : base(kasBuilder, schemeBuilder, entropyProviderFactory, macParametersBuilder, curveFactory)
        {
        }

        protected override IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> GetKasInstance(SchemeKeyNonceGenRequirement partyKeyNonceRequirements, KeyAgreementRole partyRole, KeyConfirmationRole partyKcRole, MacParameters macParameters, KasAftParametersEcc param, KasAftResultEcc result, BitString partyId)
        {
            return KasBuilder
                .WithAssurances(KasAssurance.None)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(
                    param.EccScheme, param.EccParameterSet, param.Curve
                ))
                .WithSchemeBuilder(
                    SchemeBuilder
                        .WithHashFunction(param.HashFunction)
                )
                .WithPartyId(partyId)
                .WithKeyAgreementRole(partyRole)
                .BuildKdfNoKc()
                .WithKeyLength(param.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(param.OiPattern)
                .Build();
        }
    }
}
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Ecc
{
    internal class KasAftEccTestGeneratorNoKdfNoKc : KasAftEccTestGeneratorBase
    {
        public KasAftEccTestGeneratorNoKdfNoKc(
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

        protected override IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> GetKasInstance(SchemeKeyNonceGenRequirement<EccScheme> partyKeyNonceRequirements, KeyAgreementRole partyRole, KeyConfirmationRole partyKcRole, MacParameters macParameters, KasAftParametersEcc param, KasAftResultEcc result, BitString partyId)
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
                .BuildNoKdfNoKc()
                .Build();
        }
    }
}
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.Builders.Ecc;
using NIST.CVP.Math;

namespace NIST.CVP.Orleans.Grains.Kas.Ecc
{
    internal class KasValFfcTestGeneratorNoKdfNoKc : KasValEccTestGeneratorBase
    {
        public KasValFfcTestGeneratorNoKdfNoKc(
            IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > schemeBuilder) : base(kasBuilder, schemeBuilder)
        {
        }

        protected override IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> 
            GetKasInstance(KeyAgreementRole partyRole, KeyConfirmationRole partyKcRole, MacParameters macParameters, KasValParametersEcc param, KasValResultEcc result, BitString partyId, IKdfFactory kdfFactory, INoKeyConfirmationFactory noKeyConfirmationFactory, IKeyConfirmationFactory keyConfirmationFactory)
        {
            return new KasBuilderEcc(_schemeBuilder)
                .WithPartyId(partyId)
                .WithKeyAgreementRole(partyRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(
                    param.EccScheme, param.EccParameterSet, param.Curve
                ))
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithHashFunction(param.HashFunction)
                )
                .BuildNoKdfNoKc()
                .Build();
        }
    }
}
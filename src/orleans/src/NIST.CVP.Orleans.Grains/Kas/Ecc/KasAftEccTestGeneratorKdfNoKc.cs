using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math;

namespace NIST.CVP.Orleans.Grains.Kas.Ecc
{
    internal class KasAftEccTestGeneratorKdfNoKc : KasAftEccTestGeneratorBase
    {
        public KasAftEccTestGeneratorKdfNoKc(
            IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > schemeBuilder) : base(kasBuilder, schemeBuilder)
        {
        }

        protected override IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> GetKasInstance(SchemeKeyNonceGenRequirement<EccScheme> partyKeyNonceRequirements, KeyAgreementRole partyRole, KeyConfirmationRole partyKcRole, MacParameters macParameters, KasAftParametersEcc param, KasAftResultEcc result, BitString partyId)
        {
            return _kasBuilder
                .WithAssurances(KasAssurance.None)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(
                    param.EccScheme, param.EccParameterSet, param.Curve
                ))
                .WithSchemeBuilder(
                    _schemeBuilder
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
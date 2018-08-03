using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Oracle.KAS.Ffc
{
    internal class KasAftFfcTestGeneratorKdfNoKc : KasAftFfcTestGeneratorBase
    {
        public KasAftFfcTestGeneratorKdfNoKc(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > schemeBuilder) : base(kasBuilder, schemeBuilder)
        {
        }

        protected override IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> GetKasInstance(SchemeKeyNonceGenRequirement<FfcScheme> partyKeyNonceRequirements, KeyAgreementRole partyRole, KeyConfirmationRole partyKcRole, MacParameters macParameters, KasAftParametersFfc param, KasAftResultFfc result, BitString partyId)
        {
            return _kasBuilder
                .WithAssurances(KasAssurance.None)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(
                    param.FfcScheme, param.FfcParameterSet
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
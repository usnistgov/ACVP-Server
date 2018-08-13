using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.Oracle.KAS.Ffc.Helpers;

namespace NIST.CVP.Crypto.Oracle.KAS.Ffc
{
    internal abstract class KasValFfcTestGeneratorBase : KasValTestGeneratorBase<
    KasValParametersFfc, KasValResultFfc,
    KasDsaAlgoAttributesFfc, FfcDomainParameters, FfcKeyPair>
    {
        protected KasValFfcTestGeneratorBase(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > schemeBuilder) : base(kasBuilder, schemeBuilder)
        {
        }

        protected override FfcDomainParameters GetDomainParameters(KasValParametersFfc param)
        {
            return new FfcDomainParameters(param.P, param.Q, param.G);
        }

        protected override void MangleKeys(KasValResultFfc result, KasValTestDisposition intendedDisposition, IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas)
        {
            TestDispositionHelperFfc.MangleKeys(
                result,
                intendedDisposition,
                serverKas,
                iutKas
            );
        }

        protected override void SetResultInformationFromKasProcessing(KasValParametersFfc param, KasValResultFfc result, IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas, KasResult iutResult)
        {
            TestDispositionHelperFfc.SetResultInformationFromKasProcessing(
                param,
                result,
                serverKas,
                iutKas,
                iutResult
            );
        }
    }
}

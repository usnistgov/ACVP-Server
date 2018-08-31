using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Orleans.Grains.Kas.Ecc.Helpers;

namespace NIST.CVP.Orleans.Grains.Kas.Ecc
{
    internal abstract class KasValEccTestGeneratorBase : KasValTestGeneratorBase<
    KasValParametersEcc, KasValResultEcc,
    KasDsaAlgoAttributesEcc, EccDomainParameters, EccKeyPair>
    {
        protected readonly EccCurveFactory _curveFactory;

        protected KasValEccTestGeneratorBase(
            IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > schemeBuilder) : base(kasBuilder, schemeBuilder)
        {
            _curveFactory = new EccCurveFactory();
        }

        protected override EccDomainParameters GetDomainParameters(KasValParametersEcc param)
        {
            return new EccDomainParameters(new EccCurveFactory().GetCurve(param.Curve));
        }

        protected override void MangleKeys(KasValResultEcc result, KasValTestDisposition intendedDisposition, IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas, IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas)
        {
            TestDispositionHelperEcc.MangleKeys(
                result,
                intendedDisposition,
                serverKas,
                iutKas
            );
        }

        protected override void SetResultInformationFromKasProcessing(KasValParametersEcc param, KasValResultEcc result, IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas, IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas, KasResult iutResult)
        {
            TestDispositionHelperEcc.SetResultInformationFromKasProcessing(
                param,
                result,
                serverKas,
                iutKas,
                iutResult
            );
        }
    }
}

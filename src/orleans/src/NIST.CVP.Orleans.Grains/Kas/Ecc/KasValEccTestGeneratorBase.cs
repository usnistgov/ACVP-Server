using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Kas.Ecc.Helpers;

namespace NIST.CVP.Orleans.Grains.Kas.Ecc
{
    internal abstract class KasValEccTestGeneratorBase : KasValTestGeneratorBase<
    KasValParametersEcc, KasValResultEcc,
    KasDsaAlgoAttributesEcc, EccDomainParameters, EccKeyPair>
    {
        private readonly IEccCurveFactory _curveFactory;

        protected KasValEccTestGeneratorBase(
            IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > schemeBuilder,
            IEntropyProviderFactory entropyProviderFactory,
            IMacParametersBuilder macParametersBuilder,
            IKdfFactory kdfFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            IEccCurveFactory curveFactory,
            IShaFactory shaFactory
        ) : base(kasBuilder, schemeBuilder, entropyProviderFactory, macParametersBuilder, kdfFactory, noKeyConfirmationFactory, keyConfirmationFactory, shaFactory)
        {
            _curveFactory = curveFactory;
        }

        protected override EccDomainParameters GetDomainParameters(KasValParametersEcc param)
        {
            return new EccDomainParameters(_curveFactory.GetCurve(param.Curve));
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

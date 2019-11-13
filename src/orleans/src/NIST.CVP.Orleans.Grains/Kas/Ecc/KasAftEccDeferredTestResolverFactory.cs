using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Ecc
{
    public class KasAftEccDeferredTestResolverFactory : IKasAftDeferredTestResolverFactory<KasAftDeferredParametersEcc, KasAftDeferredResult>
    {
        private readonly ISchemeBuilder<
            KasDsaAlgoAttributesEcc, 
            OtherPartySharedInformation<
                EccDomainParameters, 
                EccKeyPair
            >, 
            EccDomainParameters, 
            EccKeyPair
        > _schemeBuilder;
        private readonly IKasBuilder<
            KasDsaAlgoAttributesEcc,
            OtherPartySharedInformation<
                EccDomainParameters,
                EccKeyPair
            >,
            EccDomainParameters,
            EccKeyPair
        > _kasBuilder;
        private readonly IEccCurveFactory _curveFactory;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;

        public KasAftEccDeferredTestResolverFactory(
            ISchemeBuilder<
                KasDsaAlgoAttributesEcc, 
                OtherPartySharedInformation<
                    EccDomainParameters, 
                    EccKeyPair
                >, 
                EccDomainParameters, 
                EccKeyPair
            > schemeBuilder,
            IKasBuilder<
                KasDsaAlgoAttributesEcc,
                OtherPartySharedInformation<
                    EccDomainParameters,
                    EccKeyPair
                >,
                EccDomainParameters,
                EccKeyPair
            > kasBuilder,
            IEccCurveFactory curveFactory,
            IMacParametersBuilder macParametersBuilder,
            IEntropyProviderFactory entropyProviderFactory
        )
        {
            _schemeBuilder = schemeBuilder;
            _kasBuilder = kasBuilder;
            _curveFactory = curveFactory;
            _macParametersBuilder = macParametersBuilder;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public IKasAftDeferredTestResolver<KasAftDeferredParametersEcc, KasAftDeferredResult> GetInstance(KasMode kasMode)
        {
            switch (kasMode)
            {
                case KasMode.NoKdfNoKc:
                    return new KasAftEccDeferredTestResolverNoKdfNoKc(_curveFactory, _kasBuilder, _schemeBuilder, _macParametersBuilder, _entropyProviderFactory);
                case KasMode.KdfNoKc:
                    return new KasAftEccDeferredTestResolverKdfNoKc(_curveFactory, _kasBuilder, _schemeBuilder, _macParametersBuilder, _entropyProviderFactory);
                case KasMode.KdfKc:
                    return new KasAftEccDeferredTestResolverKdfKc(_curveFactory, _kasBuilder, _schemeBuilder, _macParametersBuilder, _entropyProviderFactory);
                default:
                    throw new ArgumentException(nameof(kasMode));
            }
        }
    }
}
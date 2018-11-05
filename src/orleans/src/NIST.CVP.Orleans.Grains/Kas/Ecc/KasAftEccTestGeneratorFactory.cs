using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Ecc
{
    public class KasAftEccTestGeneratorFactory : IKasAftTestGeneratorFactory<KasAftParametersEcc, KasAftResultEcc>
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
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly IEccCurveFactory _curveFactory;

        public KasAftEccTestGeneratorFactory(
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
            IEntropyProviderFactory entropyProviderFactory,
            IMacParametersBuilder macParametersBuilder,
            IEccCurveFactory curveFactory
        )
        {
            _schemeBuilder = schemeBuilder;
            _kasBuilder = kasBuilder;
            _entropyProviderFactory = entropyProviderFactory;
            _macParametersBuilder = macParametersBuilder;
            _curveFactory = curveFactory;
        }

        public IKasAftTestGenerator<KasAftParametersEcc, KasAftResultEcc> GetInstance(KasMode kasMode)
        {
            switch (kasMode)
            {
                case KasMode.NoKdfNoKc:
                    return new KasAftEccTestGeneratorNoKdfNoKc(_kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder, _curveFactory);
                case KasMode.KdfNoKc:
                    return new KasAftEccTestGeneratorKdfNoKc(_kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder, _curveFactory);
                case KasMode.KdfKc:
                    return new KasAftEccTestGeneratorKdfKc(_kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder, _curveFactory);
                default:
                    throw new ArgumentException(nameof(kasMode));
            }
        }
    }
}
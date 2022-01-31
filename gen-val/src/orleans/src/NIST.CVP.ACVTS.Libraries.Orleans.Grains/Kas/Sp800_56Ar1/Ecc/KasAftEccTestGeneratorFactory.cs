using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1.Ecc
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

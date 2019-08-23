using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Ecc
{
    public class KasValEccTestGeneratorFactory : IKasValTestGeneratorFactory<KasValParametersEcc, KasValResultEcc>
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
        private readonly IKdfOneStepFactory _kdfFactory;
        private readonly INoKeyConfirmationFactory _noKeyConfirmationFactory;
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;
        private readonly IEccCurveFactory _curveFactory;
        private readonly IShaFactory _shaFactory;

        public KasValEccTestGeneratorFactory(
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
            IKdfOneStepFactory kdfFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            IEccCurveFactory curveFactory,
            IShaFactory shaFactory
        )
        {
            _schemeBuilder = schemeBuilder;
            _kasBuilder = kasBuilder;
            _entropyProviderFactory = entropyProviderFactory;
            _macParametersBuilder = macParametersBuilder;
            _kdfFactory = kdfFactory;
            _noKeyConfirmationFactory = noKeyConfirmationFactory;
            _keyConfirmationFactory = keyConfirmationFactory;
            _curveFactory = curveFactory;
            _shaFactory = shaFactory;
        }

        public IKasValTestGenerator<KasValParametersEcc, KasValResultEcc> GetInstance(KasMode kasMode)
        {
            switch (kasMode)
            {
                case KasMode.NoKdfNoKc:
                    return new KasValFfcTestGeneratorNoKdfNoKc(_kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder, _kdfFactory, _noKeyConfirmationFactory, _keyConfirmationFactory, _curveFactory, _shaFactory);
                case KasMode.KdfNoKc:
                    return new KasValEccTestGeneratorKdfNoKc(_kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder, _kdfFactory, _noKeyConfirmationFactory, _keyConfirmationFactory, _curveFactory, _shaFactory);
                case KasMode.KdfKc:
                    return new KasValEccTestGeneratorKdfKc(_kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder, _kdfFactory, _noKeyConfirmationFactory, _keyConfirmationFactory, _curveFactory, _shaFactory);
                default:
                    throw new ArgumentException(nameof(kasMode));
            }
        }
    }
}

using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ecc;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Oracle.KAS.Ecc
{
    public class KasAftEccDeferredTestResolverFactory : IKasAftDeferredTestResolverFactory<KasAftDeferredParametersEcc, KasAftDeferredResult>
    {
        private readonly SchemeBuilderEcc _schemeBuilder;
        private readonly KasBuilderEcc _kasBuilder;
        private readonly EccCurveFactory _curveFactory = new EccCurveFactory();
        private readonly MacParametersBuilder _macParametersBuilder = new MacParametersBuilder();
        private readonly EntropyProviderFactory _entropyProviderFactory = new EntropyProviderFactory();

        public KasAftEccDeferredTestResolverFactory()
        {
            var shaFactory = new ShaFactory();
            var entropyProvider = new EntropyProvider(new Random800_90());
            _schemeBuilder = new SchemeBuilderEcc(
                new DsaEccFactory(shaFactory),
                new EccCurveFactory(),
                new KdfFactory(shaFactory),
                new KeyConfirmationFactory(),
                new NoKeyConfirmationFactory(),
                new OtherInfoFactory(new EntropyProvider(new Random800_90())),
                entropyProvider,
                new DiffieHellmanEcc(),
                new MqvEcc()
            );
            _kasBuilder = new KasBuilderEcc(_schemeBuilder);
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
using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Oracle.KAS.Ffc
{
    public class KasAftFfcDeferredTestResolverFactory : IKasAftDeferredTestResolverFactory<KasAftDeferredParametersFfc, KasAftDeferredResult>
    {
        private readonly SchemeBuilderFfc _schemeBuilder;
        private readonly KasBuilderFfc _kasBuilder;
        private readonly MacParametersBuilder _macParametersBuilder = new MacParametersBuilder();
        private readonly EntropyProviderFactory _entropyProviderFactory = new EntropyProviderFactory();

        public KasAftFfcDeferredTestResolverFactory()
        {
            var shaFactory = new ShaFactory();
            var entropyProvider = new EntropyProvider(new Random800_90());
            _schemeBuilder = new SchemeBuilderFfc(
                new DsaFfcFactory(shaFactory),
                new KdfFactory(shaFactory),
                new KeyConfirmationFactory(),
                new NoKeyConfirmationFactory(),
                new OtherInfoFactory(new EntropyProvider(new Random800_90())),
                entropyProvider,
                new DiffieHellmanFfc(),
                new MqvFfc()
            );
            _kasBuilder = new KasBuilderFfc(_schemeBuilder);
        }

        public IKasAftDeferredTestResolver<KasAftDeferredParametersFfc, KasAftDeferredResult> GetInstance(KasMode kasMode)
        {
            switch (kasMode)
            {
                case KasMode.NoKdfNoKc:
                    return new KasAftFfcDeferredTestResolverNoKdfNoKc(_kasBuilder, _schemeBuilder, _macParametersBuilder, _entropyProviderFactory);
                case KasMode.KdfNoKc:
                    return new KasAftFfcDeferredTestResolverKdfNoKc(_kasBuilder, _schemeBuilder, _macParametersBuilder, _entropyProviderFactory);
                case KasMode.KdfKc:
                    return new KasAftFfcDeferredTestResolverKdfKc(_kasBuilder, _schemeBuilder, _macParametersBuilder, _entropyProviderFactory);
                default:
                    throw new ArgumentException(nameof(kasMode));
            }
        }
    }
}
using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Ffc
{
    public class KasAftFfcTestGeneratorFactory : IKasAftTestGeneratorFactory<KasAftParametersFfc, KasAftResultFfc>
    {
        private readonly SchemeBuilderFfc _schemeBuilder;
        private readonly KasBuilderFfc _kasBuilder;

        public KasAftFfcTestGeneratorFactory()
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

        public IKasAftTestGenerator<KasAftParametersFfc, KasAftResultFfc> GetInstance(KasMode kasMode)
        {
            switch (kasMode)
            {
                case KasMode.NoKdfNoKc:
                    return new KasAftFfcTestGeneratorNoKdfNoKc(_kasBuilder, _schemeBuilder);
                case KasMode.KdfNoKc:
                    return new KasAftFfcTestGeneratorKdfNoKc(_kasBuilder, _schemeBuilder);
                case KasMode.KdfKc:
                    return new KasAftFfcTestGeneratorKdfKc(_kasBuilder, _schemeBuilder);
                default:
                    throw new ArgumentException(nameof(kasMode));
            }
        }
    }
}
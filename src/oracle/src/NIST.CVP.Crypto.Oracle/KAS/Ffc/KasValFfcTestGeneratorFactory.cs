using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Oracle.KAS.Ffc
{
    public class KasValFfcTestGeneratorFactory : IKasValTestGeneratorFactory<KasValParametersFfc, KasValResultFfc>
    {
        private readonly SchemeBuilderFfc _schemeBuilder;
        private readonly KasBuilderFfc _kasBuilder;

        public KasValFfcTestGeneratorFactory()
        {
            var shaFactory = new ShaFactory();
            var entropyProvider = new EntropyProvider(new Random800_90());
            _schemeBuilder = new SchemeBuilderFfc(
                new DsaFfcFactory(shaFactory),
                new KdfFactory(shaFactory),
                new KeyConfirmationFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()), new HmacFactory(new ShaFactory())),
                new NoKeyConfirmationFactory(),
                new OtherInfoFactory(new EntropyProvider(new Random800_90())),
                entropyProvider,
                new DiffieHellmanFfc(),
                new MqvFfc()
            );
            _kasBuilder = new KasBuilderFfc(_schemeBuilder);
        }

        public IKasValTestGenerator<KasValParametersFfc, KasValResultFfc> GetInstance(KasMode kasMode)
        {
            switch (kasMode)
            {
                case KasMode.NoKdfNoKc:
                    return new KasValFfcTestGeneratorNoKdfNoKc(_kasBuilder, _schemeBuilder);
                case KasMode.KdfNoKc:
                    return new KasValFfcTestGeneratorKdfNoKc(_kasBuilder, _schemeBuilder);
                case KasMode.KdfKc:
                    return new KasValFfcTestGeneratorKdfKc(_kasBuilder, _schemeBuilder);
                default:
                    throw new ArgumentException(nameof(kasMode));
            }
        }
    }
}

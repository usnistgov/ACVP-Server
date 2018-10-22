using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Builders.Ecc;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Oracle.KAS.Ecc
{
    public class KasAftEccTestGeneratorFactory : IKasAftTestGeneratorFactory<KasAftParametersEcc, KasAftResultEcc>
    {
        private readonly SchemeBuilderEcc _schemeBuilder;
        private readonly KasBuilderEcc _kasBuilder;

        public KasAftEccTestGeneratorFactory()
        {
            var shaFactory = new ShaFactory();
            var entropyProvider = new EntropyProvider(new Random800_90());
            _schemeBuilder = new SchemeBuilderEcc(
                new DsaEccFactory(shaFactory),
                new EccCurveFactory(),
                new KdfFactory(shaFactory),
                new KeyConfirmationFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()), new HmacFactory(new ShaFactory())),
                new NoKeyConfirmationFactory(),
                new OtherInfoFactory(new EntropyProvider(new Random800_90())),
                entropyProvider,
                new DiffieHellmanEcc(),
                new MqvEcc()
            );
            _kasBuilder = new KasBuilderEcc(_schemeBuilder);
        }

        public IKasAftTestGenerator<KasAftParametersEcc, KasAftResultEcc> GetInstance(KasMode kasMode)
        {
            switch (kasMode)
            {
                case KasMode.NoKdfNoKc:
                    return new KasAftEccTestGeneratorNoKdfNoKc(_kasBuilder, _schemeBuilder);
                case KasMode.KdfNoKc:
                    return new KasAftEccTestGeneratorKdfNoKc(_kasBuilder, _schemeBuilder);
                case KasMode.KdfKc:
                    return new KasAftEccTestGeneratorKdfKc(_kasBuilder, _schemeBuilder);
                default:
                    throw new ArgumentException(nameof(kasMode));
            }
        }
    }
}
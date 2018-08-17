using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Common;
using NIST.CVP.Crypto.ANSIX963;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Crypto.DRBG;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.IKEv1;
using NIST.CVP.Crypto.IKEv2;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ecc;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.KMAC;
using NIST.CVP.Crypto.ParallelHash;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Keys;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SNMP;
using NIST.CVP.Crypto.SRTP;
using NIST.CVP.Crypto.SSH;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.TLS;
using NIST.CVP.Crypto.TupleHash;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains
{
    /// <summary>
    /// Performs service injection for orleans.
    /// </summary>
    public static class ConfigureServices
    {
        public static void RegisterServices(IServiceCollection svc)
        {
            svc.AddSingleton(new LimitedConcurrencyLevelTaskScheduler(25));
            svc.AddSingleton<EntropyProviderFactory>();

            svc.AddSingleton<ModeBlockCipherFactory>();
            svc.AddSingleton<AeadModeBlockCipherFactory>();
            svc.AddSingleton<BlockCipherEngineFactory>();
            svc.AddSingleton<AesMonteCarloFactory>();
            svc.AddSingleton<TdesMonteCarloFactory>();
            svc.AddSingleton<TdesPartitionsMonteCarloFactory>();
            svc.AddSingleton<CounterFactory>();

            svc.AddSingleton<CmacFactory>();

            svc.AddSingleton<DrbgFactory>();

            svc.AddSingleton<HmacFactory>();

            svc.AddSingleton<MacParametersBuilder>();
            svc.AddSingleton<KeyConfirmationFactory>();
            svc.AddSingleton<NoKeyConfirmationFactory>();
            svc.AddSingleton<KdfFactory>();

            svc.AddSingleton<DiffieHellmanFfc>();
            svc.AddSingleton<MqvFfc>();
            svc.AddSingleton<SchemeBuilderFfc>();
            svc.AddSingleton<KasBuilderFfc>();
            svc.AddSingleton<OtherInfoFactory>();
            svc.AddSingleton<DsaFfcFactory>();

            svc.AddSingleton<DiffieHellmanEcc>();
            svc.AddSingleton<MqvEcc>();
            svc.AddSingleton<SchemeBuilderEcc>();
            svc.AddSingleton<KasBuilderEcc>();
            svc.AddSingleton<OtherInfoFactory>();
            svc.AddSingleton<DsaEccFactory>();
            svc.AddSingleton<EccCurveFactory>();

            svc.AddSingleton<EccDhComponent>();

            svc.AddSingleton<Crypto.KDF.KdfFactory>();

            svc.AddSingleton<AnsiX963Factory>();
            svc.AddSingleton<IkeV1Factory>();
            svc.AddSingleton<IkeV2Factory>();
            svc.AddSingleton<SnmpFactory>();
            svc.AddSingleton<SrtpFactory>();
            svc.AddSingleton<SshFactory>();
            svc.AddSingleton<TlsKdfFactory>();

            svc.AddSingleton<KeyWrapFactory>();

            svc.AddSingleton<SHA>();
            svc.AddSingleton<SHA_MCT>();

            svc.AddSingleton<SHA3>();
            svc.AddSingleton<SHA3_MCT>();
            svc.AddSingleton<SHAKE_MCT>();

            svc.AddSingleton<KeyBuilder>();
            svc.AddSingleton<KeyComposerFactory>();
            svc.AddSingleton<PrimeGeneratorFactory>();
            svc.AddSingleton<Rsa>();
            svc.AddSingleton<RsaVisitor>();

            svc.AddSingleton<SignatureBuilder>();
            svc.AddSingleton<PaddingFactory>();
            svc.AddSingleton<ShaFactory>();

            svc.AddSingleton<EccCurveFactory>();
            svc.AddSingleton<EccDsa>();
            svc.AddSingleton<DsaEccFactory>();

            svc.AddSingleton<PQGeneratorValidatorFactory>();
            svc.AddSingleton<GGeneratorValidatorFactory>();

            svc.AddSingleton<CSHAKE>();
            svc.AddSingleton<CSHAKE_MCT>();

            svc.AddSingleton<TupleHash>();
            svc.AddSingleton<TupleHash_MCT>();

            svc.AddSingleton<KmacFactory>();
            svc.AddSingleton<CSHAKEWrapper>();

            svc.AddSingleton<ParallelHash>();
            svc.AddSingleton<ParallelHash_MCT>();
        }
    }
}

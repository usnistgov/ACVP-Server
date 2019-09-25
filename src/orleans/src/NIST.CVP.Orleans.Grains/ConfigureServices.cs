using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Common;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto;
using NIST.CVP.Crypto.ANSIX942;
using NIST.CVP.Crypto.ANSIX963;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX942;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX963;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv2;
using NIST.CVP.Crypto.Common.KDF.Components.PBKDF;
using NIST.CVP.Crypto.Common.KDF.Components.SNMP;
using NIST.CVP.Crypto.Common.KDF.Components.SRTP;
using NIST.CVP.Crypto.Common.KDF.Components.SSH;
using NIST.CVP.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.Crypto.Common.KDF.Components.TPM;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Crypto.DRBG;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.Ed;
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
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Crypto.KMAC;
using NIST.CVP.Crypto.ParallelHash;
using NIST.CVP.Crypto.PBKDF;
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
using NIST.CVP.Crypto.TPM;
using NIST.CVP.Crypto.TupleHash;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Aead;
using NIST.CVP.Orleans.Grains.Eddsa;
using NIST.CVP.Orleans.Grains.Kas;
using NIST.CVP.Orleans.Grains.Kas.Ecc;
using NIST.CVP.Orleans.Grains.Kas.Ffc;
using NIST.CVP.Orleans.Grains.Rsa;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using NIST.CVP.Crypto.AES_FF;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Ffx;
using NIST.CVP.Crypto.KAS.Builders.Ifc;
using NIST.CVP.Crypto.KAS.FixedInfo;
using NIST.CVP.Crypto.KAS.KDF.OneStep;
using NIST.CVP.Crypto.KTS;
using NIST.CVP.Crypto.Symmetric.BlockModes.Ffx;

namespace NIST.CVP.Orleans.Grains
{
    /// <summary>
    /// Performs service injection for orleans.
    /// </summary>
    public static class ConfigureServices
    {
        public static void RegisterServices(
            IServiceCollection svc,
            IConfiguration configuration,
            OrleansConfig orleansConfig
        )
        {
            svc.AddSingleton(configuration);
            svc.AddSingleton(new LimitedConcurrencyLevelTaskScheduler(
                GetOrleansNodeMaxConcurrency(orleansConfig)
            ));
            svc.AddSingleton<IEntropyProviderFactory, EntropyProviderFactory>();
            svc.AddTransient<IRandom800_90, Random800_90>();
            svc.AddTransient<IEntropyProvider, EntropyProvider>();

            #region Orleans Registrations
            svc.AddSingleton<IAeadRunner, AeadRunner>();

            svc.AddSingleton<IEddsaKeyGenRunner, EddsaKeyGenRunner>();

            svc.AddTransient<IKasAftTestGeneratorFactory<KasAftParametersEcc, KasAftResultEcc>,
                KasAftEccTestGeneratorFactory>();
            svc.AddTransient<IKasAftTestGeneratorFactory<KasAftParametersFfc, KasAftResultFfc>,
                KasAftFfcTestGeneratorFactory>();
            svc.AddTransient<IKasAftDeferredTestResolverFactory<KasAftDeferredParametersEcc, KasAftDeferredResult>,
                KasAftEccDeferredTestResolverFactory>();
            svc.AddTransient<IKasAftDeferredTestResolverFactory<KasAftDeferredParametersFfc, KasAftDeferredResult>,
                KasAftFfcDeferredTestResolverFactory>();
            svc.AddTransient<IKasValTestGeneratorFactory<KasValParametersEcc, KasValResultEcc>,
                KasValEccTestGeneratorFactory>();
            svc.AddTransient<IKasValTestGeneratorFactory<KasValParametersFfc, KasValResultFfc>,
                KasValFfcTestGeneratorFactory>();

            svc.AddTransient<IRsaRunner, RsaRunner>();

            #endregion Orleans Registrations

            #region Crypto Registrations
            svc.AddSingleton<IBlockCipherEngineFactory, BlockCipherEngineFactory>();
            svc.AddSingleton<IModeBlockCipherFactory, ModeBlockCipherFactory>();
            svc.AddSingleton<IAeadModeBlockCipherFactory, AeadModeBlockCipherFactory>();
            svc.AddSingleton<IMonteCarloFactoryAes, AesMonteCarloFactory>();
            svc.AddSingleton<IMonteCarloFactoryTdes, TdesMonteCarloFactory>();
            svc.AddSingleton<ICounterFactory, CounterFactory>();
            svc.AddTransient<IAesFfInternals, AesFfInternals>();
            svc.AddSingleton<IFfxModeBlockCipherFactory, FfxModeBlockCipherFactory>();

            svc.AddSingleton<ICmacFactory, CmacFactory>();

            svc.AddSingleton<IDrbgFactory, DrbgFactory>();

            svc.AddSingleton<IHmacFactory, HmacFactory>();
            svc.AddSingleton<IFastHmacFactory, FastHmacFactory>();

            svc.AddSingleton<INoKeyConfirmationMacDataCreator, NoKeyConfirmationMacDataCreator>();
            svc.AddSingleton<IKeyConfirmationMacDataCreator, KeyConfirmationMacDataCreator>();
            
            svc.AddTransient<IMacParametersBuilder, MacParametersBuilder>();
            svc.AddSingleton<IKeyConfirmationFactory, KeyConfirmationFactory>();
            svc.AddSingleton<INoKeyConfirmationFactory, NoKeyConfirmationFactory>();
            svc.AddSingleton<IKdfOneStepFactory, KdfOneStepFactory>();

            svc.AddSingleton<IOtherInfoFactory, OtherInfoFactory>();

            svc.AddTransient<IPreSigVerMessageRandomizerBuilder, PreSigVerMessageRandomizerBuilder>();

            svc.AddSingleton<IDiffieHellman<FfcDomainParameters, FfcKeyPair>, DiffieHellmanFfc>();
            svc.AddSingleton<IMqv<FfcDomainParameters, FfcKeyPair>, MqvFfc>();
            svc.AddTransient<ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair>, SchemeBuilderFfc>();
            svc.AddTransient<IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair>, KasBuilderFfc>();
            svc.AddSingleton<IDsaFfcFactory, DsaFfcFactory>();

            svc.AddSingleton<IDiffieHellman<EccDomainParameters, EccKeyPair>, DiffieHellmanEcc>();
            svc.AddSingleton<IMqv<EccDomainParameters, EccKeyPair>, MqvEcc>();
            svc.AddTransient<ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair>, SchemeBuilderEcc>();
            svc.AddTransient<IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair>, KasBuilderEcc>();
            svc.AddSingleton<IDsaEccFactory, DsaEccFactory>();
            svc.AddSingleton<IEccCurveFactory, EccCurveFactory>();

            svc.AddSingleton<IEccDhComponent, EccDhComponent>();

            svc.AddSingleton<IKdfFactory, KdfFactory>();
            svc.AddSingleton<IKdfParameterVisitor, KdfParameterVisitor>();
            svc.AddSingleton<IKdfVisitor, KdfVisitor>();
            svc.AddSingleton<IFixedInfoStrategyFactory, FixedInfoStrategyFactory>();
            svc.AddSingleton<IFixedInfoFactory, FixedInfoFactory>();
            svc.AddTransient<IRsaSve, RsaSve>();
            svc.AddTransient<IRsaSveBuilder, RsaSveBuilder>();
            svc.AddSingleton<IKtsFactory, KtsFactory>();
            
            svc.AddTransient<IIfcSecretKeyingMaterialBuilder, IfcSecretKeyingMaterialBuilder>();
            svc.AddTransient<IKasIfcBuilder, KasIfcBuilder>();
            svc.AddTransient<ISchemeIfcBuilder, SchemeIfcBuilder>();
            
            svc.AddSingleton<IEdwardsCurveFactory, EdwardsCurveFactory>();
            svc.AddSingleton<IDsaEdFactory, DsaEdFactory>();

            svc.AddSingleton<Crypto.Common.KDF.IKdfFactory, Crypto.KDF.KdfFactory>();

            svc.AddSingleton<IAnsiX942Factory, AnsiX942Factory>();
            svc.AddSingleton<IAnsiX963Factory, AnsiX963Factory>();
            svc.AddSingleton<IIkeV1Factory, IkeV1Factory>();
            svc.AddSingleton<IIkeV2Factory, IkeV2Factory>();
            svc.AddSingleton<IPbKdfFactory, PbKdfFactory>();
            svc.AddSingleton<ISnmpFactory, SnmpFactory>();
            svc.AddSingleton<ISrtpFactory, SrtpFactory>();
            svc.AddSingleton<ISshFactory, SshFactory>();
            svc.AddSingleton<ITlsKdfFactory, TlsKdfFactory>();
            svc.AddSingleton<ITpmFactory, TpmFactory>();

            svc.AddSingleton<IKeyWrapFactory, KeyWrapFactory>();

            svc.AddTransient<ISHA, SHA>();
            svc.AddTransient<ISHA_MCT, SHA_MCT>();

            svc.AddTransient<ISHA3, SHA3>();
            svc.AddTransient<ISHA3_MCT, SHA3_MCT>();
            svc.AddTransient<ISHAKE_MCT, SHAKE_MCT>();

            svc.AddSingleton<IKeyGenParameterHelper, KeyGenParameterHelper>();
            svc.AddTransient<IKeyBuilder, KeyBuilder>();
            svc.AddSingleton<IKeyComposerFactory, KeyComposerFactory>();
            svc.AddSingleton<IPrimeGeneratorFactory, PrimeGeneratorFactory>();
            svc.AddTransient<IRsa, Crypto.RSA.Rsa>();
            svc.AddTransient<IRsaVisitor, RsaVisitor>();

            svc.AddTransient<ISignatureBuilder, SignatureBuilder>();
            svc.AddSingleton<IPaddingFactory, PaddingFactory>();
            svc.AddSingleton<IShaFactory, ShaFactory>();

            svc.AddSingleton<IEccCurveFactory, EccCurveFactory>();
            svc.AddSingleton<IDsaEccFactory, DsaEccFactory>();

            svc.AddSingleton<IPQGeneratorValidatorFactory, PQGeneratorValidatorFactory>();
            svc.AddSingleton<IGGeneratorValidatorFactory, GGeneratorValidatorFactory>();

            svc.AddTransient<ICSHAKE, CSHAKE>();
            svc.AddTransient<ICSHAKE_MCT, CSHAKE_MCT>();

            svc.AddTransient<ITupleHash, TupleHash>();
            svc.AddTransient<ITupleHash_MCT, TupleHash_MCT>();

            svc.AddTransient<IKmacFactory, KmacFactory>();
            svc.AddTransient<ICSHAKEWrapper, CSHAKEWrapper>();

            svc.AddTransient<IParallelHash, ParallelHash>();
            svc.AddTransient<IParallelHash_MCT, ParallelHash_MCT>();
            #endregion Crypto Registrations
        }

        private static int GetOrleansNodeMaxConcurrency(OrleansConfig orleansConfig)
        {
            var localIpAddress = GetLocalIpAddress();

            var nodeConfig = orleansConfig.OrleansNodeConfig
                .FirstOrDefault(f => f.HostName.Equals(localIpAddress, StringComparison.OrdinalIgnoreCase) ||
                                     f.HostName.Equals("localhost", StringComparison.OrdinalIgnoreCase));

            if (nodeConfig == null)
            {
                throw new Exception("Could not reconcile IP address of node. Ensure this node's IP address is listed within appsettings.[env].json under 'OrleansNodeConfig'");

                //LogManager.GetCurrentClassLogger().Warn($"Falling back to default max concurrency of {orleansConfig.FallBackMinimumCores}");
                //return orleansConfig.FallBackMinimumCores;
            }

            return nodeConfig.NumberOfCores * 2 - 2;
        }

        private static string GetLocalIpAddress()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint?.Address.ToString();
            }
        }
    }
}

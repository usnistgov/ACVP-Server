using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Config;
using NIST.CVP.ACVTS.Libraries.Common.Interfaces;
using NIST.CVP.ACVTS.Libraries.Common.Services;
using NIST.CVP.ACVTS.Libraries.Crypto;
using NIST.CVP.ACVTS.Libraries.Crypto.AES_FF;
using NIST.CVP.ACVTS.Libraries.Crypto.ANSIX942;
using NIST.CVP.ACVTS.Libraries.Crypto.ANSIX963;
using NIST.CVP.ACVTS.Libraries.Crypto.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.ConditioningComponents;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX963;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.PBKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SNMP;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SRTP;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SSH;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TPM;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Ffx;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.XTS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTR;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.KeyWrap;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.DRBG.ConditioningComponents;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Crypto.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.IKEv1;
using NIST.CVP.ACVTS.Libraries.Crypto.IKEv2;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ecc;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ffc;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ifc;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.KeyWrap;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KTS;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS;
using NIST.CVP.ACVTS.Libraries.Crypto.ParallelHash;
using NIST.CVP.ACVTS.Libraries.Crypto.PBKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.SNMP;
using NIST.CVP.ACVTS.Libraries.Crypto.SRTP;
using NIST.CVP.ACVTS.Libraries.Crypto.SSH;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.Ffx;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.XTS;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.TLS;
using NIST.CVP.ACVTS.Libraries.Crypto.TPM;
using NIST.CVP.ACVTS.Libraries.Crypto.TupleHash;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aead;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Eddsa;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1.Ecc;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1.Ffc;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa;
using IShaFactory = NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.IShaFactory;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains
{
    /// <summary>
    /// Performs service injection for orleans.
    /// </summary>
    public static class ConfigureServices
    {
        public static void RegisterServices(IConfiguration configuration, IServiceCollection svc)
        {
            svc.AddSingleton(configuration);
            svc.AddSingleton<IDbConnectionStringFactory, DbConnectionStringFactory>();
            svc.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();

            svc.Configure<EnvironmentConfig>(configuration.GetSection(nameof(EnvironmentConfig)));
            svc.Configure<OrleansConfig>(configuration.GetSection(nameof(OrleansConfig)));

            var serviceProvider = svc.BuildServiceProvider();
            var orleansConfig = serviceProvider.GetService<IOptions<OrleansConfig>>().Value;
            RegisterServices(svc, orleansConfig);
        }

        private static void RegisterServices(IServiceCollection svc, OrleansConfig orleansConfig)
        {
            svc.AddSingleton(new LimitedConcurrencyLevelTaskScheduler(
                GetOrleansNodeMaxConcurrency(orleansConfig)
            ));
            svc.AddSingleton<IEntropyProviderFactory, EntropyProviderFactory>();
            svc.AddSingleton<IRandom800_90, Random800_90>();
            svc.AddSingleton<IEntropyProvider, EntropyProvider>();

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
            svc.AddSingleton<IXtsModeBlockCipherFactory, XtsModeBlockCipherFactory>();

            svc.AddSingleton<ICmacFactory, CmacFactory>();

            svc.AddSingleton<IDrbgFactory, DrbgFactory>();
            svc.AddSingleton<IHashConditioningComponentFactory, HashConditioningComponentFactory>();
            svc.AddSingleton<IBlockCipherConditioningComponentFactory, BlockCipherConditioningComponentFactory>();

            svc.AddSingleton<IHmacFactory, HmacFactory>();

            svc.AddSingleton<INoKeyConfirmationMacDataCreator, NoKeyConfirmationMacDataCreator>();
            svc.AddSingleton<IKeyConfirmationMacDataCreator, KeyConfirmationMacDataCreator>();

            svc.AddTransient<IMacParametersBuilder, MacParametersBuilder>();
            svc.AddTransient<IKeyConfirmationFactory, KeyConfirmationFactory>(); // there is some state to KMAC instances, purposefully doing a transient rather than singleton factory in this case
            svc.AddSingleton<INoKeyConfirmationFactory, NoKeyConfirmationFactory>();
            svc.AddTransient<IKdfOneStepFactory, KdfOneStepFactory>(); // there is some state to KMAC instances, purposefully doing a transient rather than singleton factory in this case

            svc.AddSingleton<IOtherInfoFactory, OtherInfoFactory>();

            svc.AddTransient<IPreSigVerMessageRandomizerBuilder, PreSigVerMessageRandomizerBuilder>();

            svc.AddTransient<ISecretKeyingMaterialBuilder, SecretKeyingMaterialBuilder>();
            svc.AddTransient<ISchemeBuilder, SchemeBuilder>();
            svc.AddTransient<IKasBuilder, KasBuilder>();
            svc.AddSingleton<ISafePrimesGroupFactory, SafePrimesFactory>();

            svc.AddSingleton<IDiffieHellman<FfcDomainParameters, FfcKeyPair>, DiffieHellmanFfc>();
            svc.AddSingleton<IMqv<FfcDomainParameters, FfcKeyPair>, MqvFfc>();
            svc.AddTransient<
                ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>,
                    FfcDomainParameters, FfcKeyPair>, SchemeBuilderFfc>();
            svc.AddTransient<
                IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>,
                    FfcDomainParameters, FfcKeyPair>, KasBuilderFfc>();
            svc.AddSingleton<IDsaFfcFactory, DsaFfcFactory>();

            svc.AddSingleton<IDiffieHellman<EccDomainParameters, EccKeyPair>, DiffieHellmanEcc>();
            svc.AddSingleton<IMqv<EccDomainParameters, EccKeyPair>, MqvEcc>();
            svc.AddTransient<
                ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>,
                    EccDomainParameters, EccKeyPair>, SchemeBuilderEcc>();
            svc.AddTransient<
                IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>,
                    EccDomainParameters, EccKeyPair>, KasBuilderEcc>();

            svc.AddSingleton<IEccNonceProviderFactory, EccNonceProviderFactory>();
            svc.AddSingleton<IDsaEccFactory, DsaEccFactory>();
            svc.AddSingleton<IEccCurveFactory, EccCurveFactory>();

            svc.AddSingleton<IEccDhComponent, EccDhComponent>();

            svc.AddTransient<IKdfFactory, KdfFactory>(); // there is some state to KMAC instances, purposefully doing a transient rather than singleton factory in this case
            svc.AddTransient<IKdfParameterVisitor, KdfParameterVisitor>(); // there is some state to KMAC instances, purposefully doing a transient rather than singleton factory in this case
            svc.AddTransient<IKdfVisitor, KdfVisitor>(); // there is some state to KMAC instances, purposefully doing a transient rather than singleton factory in this case
            svc.AddSingleton<IKdfMultiExpansionParameterVisitor, KdfMultiExpansionParameterVisitor>();
            svc.AddSingleton<IKdfMultiExpansionVisitor, KdfMultiExpansionVisitor>();
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

            svc.AddSingleton<IHssFactory, HssFactory>();
            svc.AddSingleton<ILmsMct, LmsMct>();

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

            svc.AddTransient<IKeyBuilder, KeyBuilder>();
            svc.AddSingleton<IKeyComposerFactory, KeyComposerFactory>();
            svc.AddSingleton<IPrimeGeneratorFactory, PrimeGeneratorFactory>();
            svc.AddTransient<IRsa, Crypto.RSA.Rsa>();
            svc.AddTransient<IRsaVisitor, RsaVisitor>();
            svc.AddTransient<IMaskFactory, MaskFactory>();

            svc.AddTransient<ISignatureBuilder, SignatureBuilder>();
            svc.AddSingleton<IPaddingFactory, PaddingFactory>();
            svc.AddSingleton<IShaFactory, NativeShaFactory>();

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

            svc.AddTransient<IHkdfFactory, HkdfFactory>();
            svc.AddSingleton<ITLsKdfFactory_v1_3, TlsKdfFactoryV13>();
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

            return nodeConfig.MaxConcurrentWork; ;
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

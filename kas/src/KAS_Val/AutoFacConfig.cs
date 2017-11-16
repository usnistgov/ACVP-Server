using System;
using Autofac;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.KAS;
using NIST.CVP.Generation.KAS.FFC;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace KAS_Val
{
    public class AutofacConfig
    {
        private static IContainer _container;

        public static Action<ContainerBuilder> OverrideRegistrations;

        public static IContainer Container => _container;

        public static void IoCConfiguration(string algo)
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Generator<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<Validator<TestVectorSet, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            builder.RegisterType<EntropyProviderFactory>().AsImplementedInterfaces();
            builder.RegisterType<EntropyProvider>().AsImplementedInterfaces();

            builder.RegisterType<HmacFactory>().AsImplementedInterfaces();
            builder.RegisterType<ShaFactory>().AsImplementedInterfaces();
            builder.RegisterType<CmacFactory>().AsImplementedInterfaces();
            builder.RegisterType<AES_CCMInternals>().AsImplementedInterfaces();
            builder.RegisterType<AES_CCM>().AsImplementedInterfaces();
            builder.RegisterType<DiffieHellmanFfc>().AsImplementedInterfaces();
            builder.RegisterType<MqvFfc>().AsImplementedInterfaces();
            builder.RegisterType<SchemeBuilderFfc>().AsImplementedInterfaces();
            builder.RegisterType<KasBuilderFfc>().AsImplementedInterfaces();
            builder.RegisterType<MacParametersBuilder>().AsImplementedInterfaces();
            builder.RegisterType<KeyConfirmationFactory>().AsImplementedInterfaces();
            builder.RegisterType<NoKeyConfirmationFactory>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelInternals>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelFactory>().AsImplementedInterfaces();
            builder.RegisterType<KdfFactory>().AsImplementedInterfaces();
            builder.RegisterType<
                OtherInfoFactory<
                    FfcSharedInformation<
                        FfcDomainParameters, 
                        FfcKeyPair
                    >, 
                    FfcDomainParameters, 
                    FfcKeyPair
                >
            >().AsImplementedInterfaces();
            builder.RegisterType<DsaFfcFactory>().AsImplementedInterfaces();

            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();

            switch (algo.ToLower())
            {
                case "kas-ffc":
                    break;

                default:
                    throw new NotImplementedException("Type not supported");
            }

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();

        }
    }
}

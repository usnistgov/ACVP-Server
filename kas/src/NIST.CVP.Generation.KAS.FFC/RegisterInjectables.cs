using Autofac;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class RegisterInjectables : IRegisterInjections
    {
        public void RegisterTypes(ContainerBuilder builder, AlgoMode algoMode)
        {
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            builder.RegisterType<EntropyProviderFactory>().AsImplementedInterfaces();
            builder.RegisterType<EntropyProvider>().AsImplementedInterfaces();
            builder.RegisterType<HmacFactory>().AsImplementedInterfaces();
            builder.RegisterType<ShaFactory>().AsImplementedInterfaces();
            builder.RegisterType<CmacFactory>().AsImplementedInterfaces();
            builder.RegisterType<AES_CCMInternals>().AsImplementedInterfaces();
            builder.RegisterType<AES_CCM>().AsImplementedInterfaces();

            builder.RegisterType<MacParametersBuilder>().AsImplementedInterfaces();
            builder.RegisterType<KeyConfirmationFactory>().AsImplementedInterfaces();
            builder.RegisterType<NoKeyConfirmationFactory>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelInternals>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelFactory>().AsImplementedInterfaces();
            builder.RegisterType<KdfFactory>().AsImplementedInterfaces();

            builder.RegisterType<Generator<Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<Validator<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();

            builder.RegisterType<PqgProviderReuseGenerations>().AsImplementedInterfaces();
            builder.RegisterType<DiffieHellmanFfc>().AsImplementedInterfaces();
            builder.RegisterType<MqvFfc>().AsImplementedInterfaces();
            builder.RegisterType<SchemeBuilderFfc>().AsImplementedInterfaces();
            builder.RegisterType<KasBuilderFfc>().AsImplementedInterfaces();
            builder.RegisterType<OtherInfoFactory>().AsImplementedInterfaces();
            builder.RegisterType<DsaFfcFactory>().AsImplementedInterfaces();

            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();
        }
    }
}
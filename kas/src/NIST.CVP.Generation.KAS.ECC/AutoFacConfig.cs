using Autofac;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS.Builders.Ecc;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.KAS.ECC
{
    public static class AutoFacConfig
    {
        public static void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<Generator<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<Validator<TestVectorSet, TestCase>>().AsImplementedInterfaces();

            builder.RegisterType<DiffieHellmanEcc>().AsImplementedInterfaces();
            builder.RegisterType<MqvEcc>().AsImplementedInterfaces();
            builder.RegisterType<SchemeBuilderEcc>().AsImplementedInterfaces();
            builder.RegisterType<KasBuilderEcc>().AsImplementedInterfaces();
            builder.RegisterType<OtherInfoFactory>().AsImplementedInterfaces();
            builder.RegisterType<DsaEccFactory>().AsImplementedInterfaces();
            builder.RegisterType<EccCurveFactory>().AsImplementedInterfaces();

            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();
        }
    }
}
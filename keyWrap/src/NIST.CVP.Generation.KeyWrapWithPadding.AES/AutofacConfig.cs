using Autofac;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.KeyWrap.AES;
using NIST.CVP.Generation.KeyWrap;

namespace NIST.CVP.Generation.KeyWrapWithPadding.AES
{
    public static class AutofacConfig
    {
        /// <summary>
        /// Adds the correct types to the container
        /// </summary>
        /// <param name="builder">The builder object containg types</param>
        public static void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<AES_ECB>().AsImplementedInterfaces();
            builder.RegisterType<Validator<TestVectorSet, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();

            // This implementation comes from the current project KWP
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();

            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Generator<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory<TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
        }
    }
}

using Autofac;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.KAS.ECC.ContractResolvers;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class RegisterInjections : IRegisterInjections
    {
        public void RegisterTypes(ContainerBuilder builder, AlgoMode algoMode)
        {
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            builder.RegisterType<EntropyProviderFactory>().AsImplementedInterfaces();
            builder.RegisterType<EntropyProvider>().AsImplementedInterfaces();
            
            builder.RegisterType<Generator<Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<Validator<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();

            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactoryAsync<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestGroup, TestCase>>().AsImplementedInterfaces();

            builder.RegisterType<JsonConverterProvider>().AsImplementedInterfaces();
            builder.RegisterType<ContractResolverFactory>().AsImplementedInterfaces();
            builder.RegisterType<VectorSetSerializer<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<VectorSetDeserializer<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
        }
    }
}
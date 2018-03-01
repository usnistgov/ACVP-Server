using Autofac;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CBC
{
    public class RegisterInjections : IRegisterInjections
    {
        public void RegisterTypes(ContainerBuilder builder, AlgoMode algoMode)
        {
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            
            builder.RegisterType<Generator<Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<Validator<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();

            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();

            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();

            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
        }
    }
}

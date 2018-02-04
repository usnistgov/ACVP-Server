using Autofac;
using NIST.CVP.Common;
using NIST.CVP.Crypto.TDES_CBC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class RegisterInjections : IRegisterInjections
    {
        public void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<Generator<Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();

            builder.RegisterType<TdesCbc>().AsImplementedInterfaces();
            builder.RegisterType<TDES_CBC_MCT>().AsImplementedInterfaces();
            builder.RegisterType<MonteCarloKeyMaker>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorKnownAnswer>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();

            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();

            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();

            builder.RegisterType<Validator<TestVectorSet, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();
        }
    }
}
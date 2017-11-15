using Autofac;
using NIST.CVP.Crypto.Core;
using NIST.CVP.Crypto.TDES_CFB;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using Algo = NIST.CVP.Crypto.TDES_CFB.Algo;

namespace NIST.CVP.Generation.TDES_CFB
{
    public class AutofacConfig
    {
        public static void RegisterTypes(ContainerBuilder builder, Algo algo)
        {
            builder.RegisterType<Generator<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<Validator<TestVectorSet, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<KnownAnswerTestFactory>().AsImplementedInterfaces();
            builder.RegisterType<MonteCarloKeyMaker>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();

            builder.Register(c => ModeFactory.GetMode(algo)).As<IModeOfOperation>();
            builder.Register(c => ModeFactoryMCT.GetMode(algo)).As<IModeOfOperationMCT>();
        }
    }
}

using Autofac;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES_CFBP;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using System;


namespace NIST.CVP.Generation.TDES_CFBP
{
    public class AutofacConfig
    {
        public static Action<ContainerBuilder> OverrideRegistrations;

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
            builder.RegisterType<TestCaseGeneratorKnownAnswer>().AsImplementedInterfaces();
            builder.RegisterType<MonteCarloKeyMaker>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();

            builder.Register(c => ModeFactory.GetMode(algo)).As<ICFBPMode>();
            builder.Register(c => ModeFactoryMCT.GetMode(algo)).As<ICFBPModeMCT>();

            OverrideRegistrations?.Invoke(builder);

        }
    }
}

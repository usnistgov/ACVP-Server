using Autofac;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES_CFB;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using System;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_CFB
{
    public class AutofacConfig
    {
        public static Action<ContainerBuilder> OverrideRegistrations;

        public static void RegisterTypes(ContainerBuilder builder, AlgoMode algo)
        {
            builder.RegisterType<Generator<Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<Validator<TestVectorSet, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorKnownAnswer>().AsImplementedInterfaces();
            builder.RegisterType<MonteCarloKeyMaker>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();

            builder.Register(c => ModeFactory.GetMode(algo)).As<ICFBMode>();
            builder.Register(c => ModeFactoryMCT.GetMode(algo)).As<ICFBModeMCT>();

            OverrideRegistrations?.Invoke(builder);
        }
    }
}

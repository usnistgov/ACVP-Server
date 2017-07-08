using System;
using Autofac;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.SHA2;
using NIST.CVP.Math;

namespace SHA2_Val
{
    public class AutofacConfig
    {
        private static IContainer _container;

        public static Action<ContainerBuilder> OverrideRegistrations;

        public static IContainer Container
        {
            get { return _container; }
        }
        public static void IoCConfiguration()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Validator<TestVectorSet, TestCase>>();
            builder.RegisterType<SHA2>().AsImplementedInterfaces();
            builder.RegisterType<SHA_MCT>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();
        }
    }
}

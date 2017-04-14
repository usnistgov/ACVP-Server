using System;
using Autofac;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.SHA2;
using NIST.CVP.Math;

namespace SHA2
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

            builder.RegisterType<Generator>();

            builder.RegisterType<SHA>().AsImplementedInterfaces();
            builder.RegisterType<SHA_MCT>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();
        }
    }
}

using System;
using Autofac;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.AES_CFB128;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace AES_CFB128
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


            builder.RegisterType<Generator<Parameters, TestVectorSet>>();
            builder.RegisterType<NIST.CVP.Generation.AES_CFB128.AES_CFB128>().AsImplementedInterfaces();
            builder.RegisterType<AES_CFB128_MCT>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<KnownAnswerTestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory>().AsImplementedInterfaces();
            builder.RegisterType<KATTestGroupFactory>().AsImplementedInterfaces();
            builder.RegisterType<MCTTestGroupFactory>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelInternals>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelFactory>().AsImplementedInterfaces();

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();

        }
    }
}

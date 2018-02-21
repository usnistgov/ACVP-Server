using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NIST.CVP.Crypto.RSA2;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.RSA_DPComponent;
using NIST.CVP.Math;

namespace RSA_DP_Component
{
    public class AutofacConfig
    {
        private static IContainer _container;
        public static Action<ContainerBuilder> OverrideRegistrations;
        public static IContainer Container => _container;

        public static void IoCConfiguration()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<KeyBuilder>().AsImplementedInterfaces();
            builder.RegisterType<KeyComposerFactory>().AsImplementedInterfaces();
            builder.RegisterType<PrimeGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<Rsa>().AsImplementedInterfaces();
            builder.RegisterType<RsaVisitor>().AsImplementedInterfaces();

            builder.RegisterType<Generator<Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();

            OverrideRegistrations?.Invoke(builder);
            _container = builder.Build();
        }
    }
}

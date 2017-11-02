using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.DSA.ECC.SigGen;
using NIST.CVP.Math;

namespace ECDSA_SigGen
{
    public class AutofacConfig
    {
        public static Action<ContainerBuilder> OverrideRegistrations;
        public static IContainer Container { get; private set; }

        public static void IoCConfiguration()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Generator<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();

            OverrideRegistrations?.Invoke(builder);
            Container = builder.Build();
        }
    }
}

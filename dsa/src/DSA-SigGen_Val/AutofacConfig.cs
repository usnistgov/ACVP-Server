using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.DSA.FFC.SigGen;
using NIST.CVP.Math;

namespace DSA_SigGen_Val
{
    public class AutofacConfig
    {
        public static Action<ContainerBuilder> OverrideRegistrations;
        public static IContainer Container { get; private set; }

        public static void IoCConfiguration()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Generator<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<Validator<TestVectorSet, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();

            OverrideRegistrations?.Invoke(builder);
            Container = builder.Build();
        }
    }
}

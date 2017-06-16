using System;
using Autofac;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.KeyWrap;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace KeyWrap_Val
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
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<NIST.CVP.Generation.KeyWrap.Generator<Parameters, TestVectorSet>>();
            builder.RegisterType<NIST.CVP.Generation.KeyWrap.Validator<TestVectorSet, TestCase>>();
            builder.RegisterType<EntropyProviderFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelInternals>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelFactory>().AsImplementedInterfaces();

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();

        }
    }
}

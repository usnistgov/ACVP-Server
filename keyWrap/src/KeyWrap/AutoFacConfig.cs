using System;
using Autofac;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.KeyWrap;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;


namespace KeyWrap
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
            
            builder.RegisterType<NIST.CVP.Crypto.AES.Generator<Parameters, TestVectorSet>>();
            builder.RegisterType<EntropyProviderFactory>().AsImplementedInterfaces();
            builder.RegisterType<AES_ECB>().AsImplementedInterfaces();
            builder.RegisterType<KeyWrapFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory>().AsImplementedInterfaces();
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

using System;
using Autofac;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Crypto.TDES_ECB;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.KeyWrap;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Generation.Core;

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
        public static void IoCConfiguration(string algo)
        {
            ContainerBuilder builder = new ContainerBuilder();

         
            builder.RegisterType<EntropyProviderFactory>().AsImplementedInterfaces();
            builder.RegisterType<KeyWrapFactory>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelInternals>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelFactory>().AsImplementedInterfaces();

            switch (algo)
            {
                case "AES-KW":
                    builder.RegisterType<AES_ECB>().AsImplementedInterfaces();
                    builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
                    builder.RegisterType<Generator<Parameters, TestVectorSet>>().AsImplementedInterfaces();
                    builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
                    builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
                    builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
                    builder.RegisterType<TestCaseGeneratorFactoryFactory>().AsImplementedInterfaces();
                    builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
                    builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
                    break;
                case "TDES-KW":
                    builder.RegisterType<TDES_ECB>().AsImplementedInterfaces();
                    builder.RegisterType<ParameterValidatorTdes>().AsImplementedInterfaces();
                    builder.RegisterType<Generator<ParametersTdes, TestVectorSetTdes>>().AsImplementedInterfaces();
                    builder.RegisterType<TestVectorFactory<ParametersTdes, TestVectorSetTdes>>().AsImplementedInterfaces();
                    builder.RegisterType<ParameterParser<ParametersTdes>>().AsImplementedInterfaces();
                    builder.RegisterType<TestCaseGeneratorFactoryTdes>().AsImplementedInterfaces();
                    builder.RegisterType<TestCaseGeneratorFactoryFactoryTdes>().AsImplementedInterfaces();
                    builder.RegisterType<TestCaseValidatorFactoryTdes>().AsImplementedInterfaces();
                    builder.RegisterType<TestGroupGeneratorFactoryTdes>().AsImplementedInterfaces();
                    break;
                default:
                    throw new NotImplementedException("Needs to be using enums to avoid this");
                    break;
            }
            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();

        }
    }
}

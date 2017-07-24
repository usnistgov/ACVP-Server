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

        public static void IoCConfiguration(string algo)
        {
            ContainerBuilder builder = new ContainerBuilder();


            builder.RegisterType<EntropyProviderFactory>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelInternals>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelFactory>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();


            switch (algo)
            {
                case "AES-KW":
                    builder.RegisterType<Validator<TestVectorSet, TestCase>>().AsImplementedInterfaces();
                    builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();
                    builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
                    builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
                    builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
                    builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
                    break;
                case "TDES-KW":
                    builder.RegisterType<Validator<TestVectorSetTdes, TestCaseTdes>>().AsImplementedInterfaces();
                    builder.RegisterType<TestReconstitutorTdes>().AsImplementedInterfaces();
                    builder.RegisterType<TestCaseValidatorFactoryTdes>().AsImplementedInterfaces();
                    builder.RegisterType<ParameterValidatorTdes>().AsImplementedInterfaces();
                    builder.RegisterType<ResultValidator<TestCaseTdes>>().AsImplementedInterfaces();
                    builder.RegisterType<ParameterParser<ParametersTdes>>().AsImplementedInterfaces();

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
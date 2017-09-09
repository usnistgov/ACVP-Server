using System;
using Autofac;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Generation.CMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace CMAC_AES_Val
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

            //builder.RegisterType<Generator<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            
            builder.RegisterType<CmacFactory>().AsImplementedInterfaces();
            //builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            
            //builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            //builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            
            //builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelInternals>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelFactory>().AsImplementedInterfaces();

            switch (algo)
            {
                case "CMAC-AES":
                    NIST.CVP.Generation.CMAC.AES.AutofacConfig.RegisterTypes(builder);
                    break;

                case "CMAC-TDES":
                    NIST.CVP.Generation.CMAC.TDES.AutofacConfig.RegisterTypes(builder);

                    break;

                default:
                    throw new NotImplementedException("Type not supported");
            }

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();

        }
    }
}

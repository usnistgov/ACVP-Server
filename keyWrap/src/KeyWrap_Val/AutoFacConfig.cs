using System;
using Autofac;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace KeyWrap_Val
{
    public class AutofacConfig
    {
        private static IContainer _container;

        public static Action<ContainerBuilder> OverrideRegistrations;

        public static IContainer Container => _container;

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

                    NIST.CVP.Generation.KeyWrap.AES.AutofacConfig.RegisterTypes(builder);
                    break;
                case "TDES-KW":
                    NIST.CVP.Generation.KeyWrap.TDES.AutofacConfig.RegisterTypes(builder);

                    break;
                default:
                    throw new NotImplementedException("Type not supported");
            }

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();




        }
    }
}
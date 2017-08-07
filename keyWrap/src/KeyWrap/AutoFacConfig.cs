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

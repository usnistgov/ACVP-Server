using System;
using Autofac;
using NIST.CVP.Crypto.Core;
using NIST.CVP.Crypto.TDES_CFB;
using Algo = NIST.CVP.Crypto.TDES_CFB.Algo;

namespace TDES_CFB_Val
{
    public class AutofacConfig
    {
        private static IContainer _container;

        public static Action<ContainerBuilder> OverrideRegistrations;

        public static IContainer Container
        {
            get { return _container; }
        }
        public static void IoCConfiguration(Algo algo)
        {
            ContainerBuilder builder = new ContainerBuilder();

            //builder.RegisterType<TdesCfbFactory>().AsImplementedInterfaces();

            switch (algo)
            {
                case Algo.TDES_CFB1:
                case Algo.TDES_CFB8:
                case Algo.TDES_CFB64:
                    NIST.CVP.Generation.TDES_CFB.AutofacConfig.RegisterTypes(builder, algo);
                    break;

                default:
                    throw new NotImplementedException("Type not supported");
            }

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();

        }
    }
}

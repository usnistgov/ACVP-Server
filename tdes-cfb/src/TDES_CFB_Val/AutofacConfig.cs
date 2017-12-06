using System;
using Autofac;
using NIST.CVP.Crypto.Common;



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
                case Algo.TDES_CFBP1:
                case Algo.TDES_CFBP8:
                case Algo.TDES_CFBP64:
                    NIST.CVP.Generation.TDES_CFBP.AutofacConfig.RegisterTypes(builder, algo);
                    break;

                default:
                    throw new NotImplementedException("Type not supported");
            }

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();

        }
    }
}

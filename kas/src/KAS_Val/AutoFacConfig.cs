using System;
using Autofac;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Math;

namespace KAS_Val
{
    public class AutofacConfig
    {
        private static IContainer _container;

        public static Action<ContainerBuilder> OverrideRegistrations;

        public static IContainer Container => _container;

        public static void IoCConfiguration(string algo)
        {
            ContainerBuilder builder = new ContainerBuilder();

            switch (algo.ToLower())
            {
                case "KAS-FFC":
                    break;

                default:
                    throw new NotImplementedException("Type not supported");
            }

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();

        }
    }
}

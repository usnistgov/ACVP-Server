using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace KDFComponent
{
    public class AutofacConfig
    {
        public static Action<ContainerBuilder> OverrideRegistrations;
        public static IContainer Container { get; private set; }

        public static void IoCConfiguration(string algo)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EntropyProviderFactory>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();

            switch (algo.ToLower())
            {
                case "ikev1":
                    NIST.CVP.Generation.IKEv1.AutofacConfig.RegisterTypes(builder);
                    break;

                case "ikev2":
                    NIST.CVP.Generation.IKEv2.AutofacConfig.RegisterTypes(builder);
                    break;

                case "tls":
                    NIST.CVP.Generation.TLS.AutofacConfig.RegisterTypes(builder);
                    break;

                case "ssh":
                    NIST.CVP.Generation.SSH.AutofacConfig.RegisterTypes(builder);
                    break;

                default:
                    throw new NotImplementedException("Type not supported");
            }

            OverrideRegistrations?.Invoke(builder);
            Container = builder.Build();
        }
    }
}

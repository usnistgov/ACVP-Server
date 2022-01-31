using Autofac;
using Microsoft.Extensions.Logging;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public class RegisterInjections : IRegisterInjections
    {
        public void RegisterTypes(ContainerBuilder builder, AlgoMode algoMode)
        {
            builder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>()
                .SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            builder.RegisterType<LoggerFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<LocalOrleansClientClustering>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ClusterClientFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<Oracle>().AsImplementedInterfaces().SingleInstance();
        }
    }
}

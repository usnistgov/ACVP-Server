using System;
using System.Net;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Enums;
using Orleans;

namespace NIST.CVP.Crypto.Oracle.ExtensionMethods
{
    public static class ClientHostBuilderExtensions
    {
        public static IClientBuilder ConfigureClustering(
            this IClientBuilder builder, 
            OrleansConfig orleansConfig,
            EnvironmentConfig environmentConfig
        )
        {
            switch (environmentConfig.Name)
            {
                case Environments.Local:
                    builder.UseLocalhostClustering();
                    break;
                case Environments.Dev:
                case Environments.Test:
                case Environments.Demo:
                case Environments.Prod:
                    var primarySiloEndpoint = new IPEndPoint(
                        IPAddress.Parse(orleansConfig.OrleansServerIp), orleansConfig.OrleansGatewayPort
                    );
                    builder.UseStaticClustering(primarySiloEndpoint);
                    break;
                default:
                    throw new ArgumentException($"invalid {nameof(environmentConfig)}");
            }

            return builder;
        }
    }
}
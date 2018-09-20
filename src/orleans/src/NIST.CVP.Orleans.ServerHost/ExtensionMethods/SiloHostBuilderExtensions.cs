using System;
using System.Net;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Enums;
using Orleans.Configuration;
using Orleans.Hosting;

namespace NIST.CVP.Orleans.ServerHost.ExtensionMethods
{
    public static class SiloHostBuilderExtensions
    {
        public static ISiloHostBuilder ConfigureClustering(
            this ISiloHostBuilder builder, 
            OrleansConfig orleansConfig,
            EnvironmentConfig environmentConfig
        )
        {
            switch (environmentConfig.Name)
            {
                case Environments.Local:
                    builder.Configure<EndpointOptions>(options =>
                    {
                        options.AdvertisedIPAddress = IPAddress.Loopback;
                    });
                    builder.UseLocalhostClustering();
                    break;
                case Environments.Dev:
                case Environments.Test:
                case Environments.Demo:
                case Environments.Prod:
                    var primarySiloEndpoint = new IPEndPoint(
                        IPAddress.Parse(orleansConfig.OrleansServerIp), orleansConfig.OrleansGatewayPort
                    );
                    builder.Configure<EndpointOptions>(options =>
                    {
                        options.AdvertisedIPAddress = primarySiloEndpoint.Address;
                        options.GatewayPort = orleansConfig.OrleansGatewayPort;
                        options.SiloPort = orleansConfig.OrleansSiloPort;
                    });
                    builder.UseDevelopmentClustering(primarySiloEndpoint);
                    break;
                default:
                    throw new ArgumentException($"invalid {nameof(environmentConfig)}");
            }

            return builder;
        }
    }
}

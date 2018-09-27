using System;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Logging;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Enums;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Logging;

namespace NIST.CVP.Orleans.ServerHost.ExtensionMethods
{
    /// <summary>
    /// <see cref="ISiloHostBuilder"/> extension methods for configuration based on
    /// environment.
    /// </summary>
    public static class SiloHostBuilderExtensions
    {

        /// <summary>
        /// Configures endpoints based on <see cref="orleansConfig"/> and <see cref="environmentConfig"/>.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloHostBuilder"/></param>
        /// <param name="orleansConfig">The loaded Orleans configuration</param>
        /// <param name="environmentConfig">The loaded environment configuraiton</param>
        /// <returns><see cref="ISiloHostBuilder"/> with updated configuration.</returns>
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
                        IPAddress.Parse(orleansConfig.OrleansNodeIps.First()), orleansConfig.OrleansSiloPort
                    );
                    builder.UseDevelopmentClustering(primarySiloEndpoint);
                    builder.ConfigureEndpoints(
                        siloPort: orleansConfig.OrleansSiloPort, 
                        gatewayPort: orleansConfig.OrleansGatewayPort
                    );
                    break;
                default:
                    throw new ArgumentException($"invalid {nameof(environmentConfig)}");
            }

            return builder;
        }

        /// <summary>
        /// Configures logging based on <see cref="orleansConfig"/> and <see cref="environmentConfig"/>.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloHostBuilder"/></param>
        /// <param name="orleansConfig">The loaded Orleans configuration</param>
        /// <param name="environmentConfig">The loaded environment configuraiton</param>
        /// <returns><see cref="ISiloHostBuilder"/> with updated configuration.</returns>
        public static ISiloHostBuilder ConfigureLogging(this ISiloHostBuilder builder, OrleansConfig orleansConfig, EnvironmentConfig environmentConfig)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.SetMinimumLevel(orleansConfig.MinimumLogLevel);
                
                if (orleansConfig.UseConsoleLogging)
                {
                    logging.AddConsole();
                }

                if (orleansConfig.UseFileLogging)
                {
                    logging.AddProvider(new FileLoggerProvider($"{DateTime.UtcNow:yyyy-MM-dd_Hmm}_{environmentConfig.Name}.log"));
                }
            });
            
            return builder;
        }
    }
}

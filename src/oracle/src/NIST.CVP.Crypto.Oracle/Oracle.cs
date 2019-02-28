using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Orleans.Grains.Interfaces;
using Orleans;
using Orleans.Configuration;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Enums;
using NIST.CVP.Crypto.Oracle.Exceptions;
using NIST.CVP.Common.Interfaces;
using Orleans.Hosting;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle : IOracle
    {
        private const int TimeoutSeconds = 60;

        private readonly IOptions<EnvironmentConfig> _environmentConfig;
        private readonly IOptions<OrleansConfig> _orleansConfig;
        private readonly IClusterClient _clusterClient;
        private readonly string _orleansConnectionString;

        public Oracle(
            IDbConnectionStringFactory dbConnectionStringFactory,
            IOptions<EnvironmentConfig> environmentConfig,
            IOptions<OrleansConfig> orleansConfig
        )
        {
            _orleansConnectionString = dbConnectionStringFactory
                .GetConnectionString(Constants.OrleansConnectionString);
            _environmentConfig = environmentConfig;
            _orleansConfig = orleansConfig;

            _clusterClient = InitializeClient().GetAwaiter().GetResult();
        }

        private async Task<IClusterClient> InitializeClient()
        {
            var orleansConfig = _orleansConfig.Value;

            int initializeCounter = 0;
            
            var initSucceed = false;
            while (!initSucceed)
            {
                try
                {
                    var clientBuilder = new ClientBuilder()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = orleansConfig.ClusterId;
                            options.ServiceId = Constants.ServiceId;
                        })
                        .Configure<ClientMessagingOptions>(opts =>
                        {
                            opts.ResponseTimeout = TimeSpan.FromSeconds(TimeoutSeconds);
                        })
                        .ConfigureApplicationParts(parts =>
                        {
                            parts.AddApplicationPart(typeof(IGrainInterfaceMarker).Assembly).WithReferences();
                        });

                    ConfigureClustering(clientBuilder);
                    var client = clientBuilder.Build();

                    await client.Connect();
                    initSucceed = client.IsInitialized;

                    if (initSucceed)
                    {
                        return client;
                    }
                }
                catch (Exception)
                {
                    initSucceed = false;
                }

                if (initializeCounter++ > 10)
                {
                    throw new OrleansInitializationException();
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

            return null;
        }

        private void ConfigureClustering(IClientBuilder builder)
        {
            switch (_environmentConfig.Value.Name)
            {
                case Environments.Local:
                    builder.UseLocalhostClustering();
                    break;
                case Environments.Tc:
                case Environments.Prod: // TODO Bad!
                    List<IPEndPoint> endpoints = new List<IPEndPoint>();
                    foreach (var endpoint in _orleansConfig.Value.OrleansNodeConfig.Select(s => s.HostName))
                    {
                        endpoints.Add(new IPEndPoint(
                            IPAddress.Parse(endpoint), _orleansConfig.Value.OrleansGatewayPort
                        ));
                    }
                    builder.UseStaticClustering(endpoints.ToArray());
                    break;
                default:
                    builder.UseAdoNetClustering(options =>
                    {
                        options.Invariant = "System.Data.SqlClient";
                        options.ConnectionString = _orleansConnectionString;
                    });
                    break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Enums;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Crypto.Oracle.Exceptions;
using NIST.CVP.Orleans.Grains.Interfaces;
using NLog;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using ILogger = NLog.ILogger;

namespace NIST.CVP.Crypto.Oracle
{
    public class ClusterClientFactory : IClusterClientFactory
    {
        private const int TIMEOUT_SECONDS = 60;
        
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _orleansConnectionString;
        private readonly IOptions<EnvironmentConfig> _environmentConfig;
        private readonly IOptions<OrleansConfig> _orleansConfig;
        
        private IClusterClient _clusterClient;
        private int _initializeCount;
        
        public ClusterClientFactory(IDbConnectionStringFactory dbConnectionStringFactory, IOptions<EnvironmentConfig> environmentConfig, IOptions<OrleansConfig> orleansConfig)
        {
            _orleansConnectionString = dbConnectionStringFactory
                .GetConnectionString(Constants.OrleansConnectionString);
            _environmentConfig = environmentConfig;
            _orleansConfig = orleansConfig;
        }

        public async Task<IClusterClient> Get()
        {
            if (_clusterClient == null)
            {
                _initializeCount++;
                _logger.Info($"Initializing Orleans cluster client. Client has been initialized {_initializeCount} times.");
                _clusterClient = await InitializeClient();
            }

            return _clusterClient;
        }

        public async Task ResetClient()
        {
            _logger.Info("Closing Orleans cluster client.");
            await _clusterClient.Close();
            _clusterClient = null;
        }

        private async Task<IClusterClient> InitializeClient()
        {
            var orleansConfig = _orleansConfig.Value;
            var initializeCounter = 0;
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
                            opts.ResponseTimeout = TimeSpan.FromSeconds(TIMEOUT_SECONDS);
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

        /// <summary>
        /// Configure the <see cref="IClientBuilder"/> to use the appropriate type of clustering,
        /// based on the environment.
        /// </summary>
        /// <param name="builder">The <see cref="IClientBuilder"/> to manipulate.</param>
        private void ConfigureClustering(IClientBuilder builder)
        {
            switch (_environmentConfig.Value.Name)
            {
                case Environments.Local:
                    builder.UseLocalhostClustering();
                    break;
                case Environments.Tc:
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
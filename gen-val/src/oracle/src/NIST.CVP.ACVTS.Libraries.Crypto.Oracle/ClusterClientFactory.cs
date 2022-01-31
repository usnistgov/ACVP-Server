using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NIST.CVP.ACVTS.Libraries.Common.Config;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Exceptions;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces;
using NLog;
using Orleans;
using Orleans.Configuration;
using ILogger = NLog.ILogger;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public class ClusterClientFactory : IClusterClientFactory
    {
        private const int TIMEOUT_SECONDS = 60;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IOrleansClientClustering _orleansClientClustering;
        private readonly IOptions<OrleansConfig> _orleansConfig;

        private IClusterClient _clusterClient;

        public ClusterClientFactory(IOrleansClientClustering orleansClientClustering, IOptions<OrleansConfig> orleansConfig)
        {
            _orleansClientClustering = orleansClientClustering;
            _orleansConfig = orleansConfig;
        }

        public async Task<IClusterClient> Get()
        {
            if (_clusterClient == null || !_clusterClient.IsInitialized)
            {
                _logger.Info($"Attempting to initialize Orleans cluster client.");
                _clusterClient = await InitializeClient();
            }

            return _clusterClient;
        }

        public async Task ResetClient()
        {
            if (_clusterClient != null && _clusterClient.IsInitialized)
            {
                _logger.Info("Closing Orleans cluster client.");
                await _clusterClient.Close();
                _clusterClient = null;
            }
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

                    _orleansClientClustering.ConfigureClustering(clientBuilder);
                    var client = clientBuilder.Build();

                    await client.Connect();
                    initSucceed = client.IsInitialized;

                    if (initSucceed)
                    {
                        return client;
                    }
                }
                catch (Exception e)
                {
                    initSucceed = false;
                    _logger.Warn($"Failed initializing Orleans client connection on attempt {initializeCounter}.{Environment.NewLine}{e}");
                }

                if (initializeCounter++ > 10)
                {
                    throw new OrleansInitializationException();
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

            return null;
        }
    }
}

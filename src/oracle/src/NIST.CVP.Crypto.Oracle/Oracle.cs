using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces;
using Orleans;
using Orleans.Configuration;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Crypto.Oracle.Exceptions;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle : IOracle
    {
        private const int TimeoutSeconds = 60;

        private readonly IOptions<EnvironmentConfig> _environmentConfig;
        private readonly IOptions<PoolConfig> _poolConfig;
        private readonly IOptions<OrleansConfig> _orleansConfig;
        private readonly IClusterClient _clusterClient;
        
        public Oracle(
            IOptions<EnvironmentConfig> environmentConfig,
            IOptions<PoolConfig> poolConfig,
            IOptions<OrleansConfig> orleansConfig
        )
        {
            _environmentConfig = environmentConfig;
            _poolConfig = poolConfig;
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
                    var client = new ClientBuilder()
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
                        })
                        .ConfigureClustering(_orleansConfig.Value, _environmentConfig.Value)
                        .Build();
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
    }
}

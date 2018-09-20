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

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle : IOracle
    {
        private readonly Random800_90 _rand = new Random800_90();

        private const int TimeoutSeconds = 60;
        
        private readonly IOptions<PoolConfig> _poolConfig;
        private readonly IOptions<OrleansConfig> _orleansConfig;
        private readonly IClusterClient _clusterClient;
        
        public Oracle(
            IOptions<PoolConfig> poolConfig,
            IOptions<OrleansConfig> orleansConfig
        )
        {
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
                    var ipEndpoint = new IPEndPoint(
                        IPAddress.Parse(orleansConfig.OrleansServerIp), orleansConfig.OrleansGatewayPort
                    );
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
                        // TODO need to make this properly configurable based on environment
                        .UseLocalhostClustering()
                        //.UseStaticClustering(ipEndpoint)
                        .ConfigureApplicationParts(parts =>
                        {
                            parts.AddApplicationPart(typeof(IGrainInterfaceMarker).Assembly).WithReferences();
                        })
                        //.ConfigureLogging(logging => logging.AddConsole())
                        //Depends on your application requirements, you can configure your client with other stream providers, which can provide other features, 
                        //such as persistence or recoverability. For more information, please see http://dotnet.github.io/orleans/Documentation/Orleans-Streams/Stream-Providers.html
                        //.AddSimpleMessageStreamProvider(Constants.ChatRoomStreamProvider)
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
                    return null;
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

            return null;
        }
    }
}

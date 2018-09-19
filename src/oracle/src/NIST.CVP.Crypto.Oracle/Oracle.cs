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
        private static readonly IClusterClient _clusterClient;
        
        static Oracle()
        {
            _clusterClient = InitializeClient().GetAwaiter().GetResult();
        }

        public Oracle(
            IOptions<PoolConfig> poolConfig
        )
        {
            _poolConfig = poolConfig;
        }

        private static async Task<IClusterClient> InitializeClient()
        {
            int initializeCounter = 0;

            var initSucceed = false;
            while (!initSucceed)
            {
                try
                {
                    var ipEndpoint = new IPEndPoint(IPAddress.Parse("10.0.0.2"), 30000);
                    var client = new ClientBuilder()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = Constants.ClusterId;
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

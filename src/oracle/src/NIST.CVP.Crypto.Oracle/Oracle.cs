using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Enums;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle : IOracle
    {
        private readonly Random800_90 _rand = new Random800_90();

        private const double GCM_FAIL_RATIO = .25;
        private const double XPN_FAIL_RATIO = .25;
        private const double CMAC_FAIL_RATIO = .25;
        private const double KEYWRAP_FAIL_RATIO = .2;
        private const double KMAC_FAIL_RATIO = .5;
        private const int RSA_PUBLIC_EXPONENT_BITS_MIN = 32;
        private const int RSA_PUBLIC_EXPONENT_BITS_MAX = 64;

        // TODO configurable concurrency through config file and/or GenValAppRunner parameter
        // Task scheduler should be an interim step until orleans is implemented, at which
        // point it will be managing tasks.  Currently, without limiting the tasks enqueued,
        // you can potentially cap out memory usage w/o being able to complete tasks.
        private static readonly LimitedConcurrencyLevelTaskScheduler _scheduler;
        private static readonly TaskFactory _taskFactory;
        private static readonly IClusterClient _clusterClient;
        
        static Oracle()
        {
            _scheduler = new LimitedConcurrencyLevelTaskScheduler(3);
            _taskFactory = new TaskFactory(_scheduler);

            _clusterClient = InitializeClient().GetAwaiter().GetResult();
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
                            //opts.ResponseTimeout = TimeSpan.FromMinutes(TimeoutMinutes);
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

        /// <summary>
        /// Handles polling a <see cref="IPollableOracleGrain{TResult}"/> until a result is available,
        /// then returns that result as a <see cref="Task{TResult}"/>
        /// </summary>
        /// <typeparam name="TResult">The type of returned result</typeparam>
        /// <param name="pollableGrain">The pollable grain.</param>
        /// <returns><see cref="Task{TResult}"/></returns>
        protected async Task<TResult> PollWorkUntilCompleteAsync<TResult>(
            IPollableOracleGrain<TResult> pollableGrain
        )
        {
            while (true)
            {
                var state = await pollableGrain.CheckStatusAsync();

                if (state == GrainState.CompletedWork)
                {
                    return await pollableGrain.GetResultAsync();
                }

                await Task.Delay(Constants.TaskPollingSeconds);
            }
        }
    }
}

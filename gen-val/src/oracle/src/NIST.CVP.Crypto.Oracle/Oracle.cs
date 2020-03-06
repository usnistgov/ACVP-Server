using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Enums;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Oracle.Exceptions;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Crypto.Oracle.Models;
using NIST.CVP.Orleans.Grains.Interfaces;
using NLog;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle : IOracle, IDisposable
    {
        private const int TimeoutSeconds = 60;

        private readonly IOptions<EnvironmentConfig> _environmentConfig;
        private readonly IOptions<OrleansConfig> _orleansConfig;
        private readonly IClusterClient _clusterClient;
        private readonly string _orleansConnectionString;

        protected readonly ILogger ThisLogger = LogManager.GetCurrentClassLogger();

        protected virtual int LoadSheddingRetries { get; }

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
            LoadSheddingRetries = _orleansConfig.Value.TimeoutRetryAttempts;

            _clusterClient = InitializeClient().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Destructor, close the cluster client connection.
        /// </summary>
        ~Oracle()
        {
            Dispose(false);
        }

        /// <summary>
        /// Get an observer grain from the cluster, and subscribe for updates.
        /// </summary>
        /// <typeparam name="TGrain">The grain type.</typeparam>
        /// <typeparam name="TGrainResultType">The result type.</typeparam>
        /// <returns>Task representing the Observer of type <see cref="TGrain"/> with result type of <see cref="TGrainResultType"/></returns>
        /// <exception cref="OrleansInitializationException">Thrown when the Orleans client is not initialized.</exception>
        private async Task<OracleObserverGrain<TGrain, TGrainResultType>>
            GetObserverGrain<TGrain, TGrainResultType>()
            where TGrain : IGrainObservable<TGrainResultType>, IGrainWithGuidKey
        {
            if (_clusterClient == null)
            {
                throw new OrleansInitializationException();
            }

            var grain = _clusterClient.GetGrain<TGrain>(Guid.NewGuid());

            var observer = new OracleGrainObserver<TGrainResultType>();
            var observerReference =
                await GrainInvokeRetryWrapper.WrapGrainCall(_clusterClient.CreateObjectReference<IGrainObserver<TGrainResultType>>, observer, LoadSheddingRetries);
            await GrainInvokeRetryWrapper.WrapGrainCall(grain.InitialSubscribe, observerReference, LoadSheddingRetries);

            return new OracleObserverGrain<TGrain, TGrainResultType>(grain, observer, observerReference, LoadSheddingRetries);
        }

        /// <summary>
        /// Initialize the <see cref="IClusterClient"/> for communication to the Orleans cluster.
        /// </summary>
        /// <returns><see cref="Task"/> representing the <see cref="IClusterClient"/></returns>
        /// <exception cref="OrleansInitializationException"></exception>
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

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _clusterClient?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

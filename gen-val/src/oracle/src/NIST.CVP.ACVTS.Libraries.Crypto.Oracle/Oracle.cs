using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NIST.CVP.ACVTS.Libraries.Common.Config;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Exceptions;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Models;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces;
using NLog;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public partial class Oracle : IOracle
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IClusterClientFactory _clusterClientFactory;
        private readonly IRandom800_90 _random;

        private IClusterClient _clusterClient;

        protected virtual int LoadSheddingRetries { get; }

        public Oracle(
            IClusterClientFactory clusterClientFactory,
            IOptions<OrleansConfig> orleansConfig,
            IRandom800_90 random)
        {
            _clusterClientFactory = clusterClientFactory;
            _random = random;
            LoadSheddingRetries = orleansConfig.Value.TimeoutRetryAttempts;
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

        public async Task InitializeClusterClient()
        {
            _clusterClient = await _clusterClientFactory.Get();
        }

        public Task CloseClusterClient()
        {
            return _clusterClientFactory.ResetClient();
        }
    }
}

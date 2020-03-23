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
    public partial class Oracle : IOracle
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IClusterClientFactory _clusterClientFactory;
        
        private IClusterClient _clusterClient;

        protected virtual int LoadSheddingRetries { get; }

        public Oracle(
            IClusterClientFactory clusterClientFactory,
            IOptions<OrleansConfig> orleansConfig
        )
        {
            _clusterClientFactory = clusterClientFactory;
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

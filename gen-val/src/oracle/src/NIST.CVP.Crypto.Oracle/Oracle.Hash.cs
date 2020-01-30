using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Aes;
using NIST.CVP.Orleans.Grains.Interfaces.Cshake;
using NIST.CVP.Orleans.Grains.Interfaces.Exceptions;
using NIST.CVP.Orleans.Grains.Interfaces.Hash;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<HashResult> GetShaCaseAsync(ShaParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverShaCaseGrain, HashResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetShaCaseAsync(param);
            }
        }

        public async Task<HashResult> GetSha3CaseAsync(Sha3Parameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverSha3CaseGrain, HashResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetSha3CaseAsync(param);
            }
        }

        public async Task<CShakeResult> GetCShakeCaseAsync(CShakeParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverCShakeCaseGrain, CShakeResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetCShakeCaseAsync(param);
            }
        }

        public async Task<ParallelHashResult> GetParallelHashCaseAsync(ParallelHashParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverParallelHashCaseGrain, ParallelHashResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetParallelHashCaseAsync(param);
            }
        }

        public async Task<TupleHashResult> GetTupleHashCaseAsync(TupleHashParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverTupleHashCaseGrain, TupleHashResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetTupleHashCaseAsync(param);
            }
        }

        public virtual async Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverShaMctCaseGrain, MctResult<HashResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetShaMctCaseAsync(param);
            }
        }

        public virtual async Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverSha3MctCaseGrain, MctResult<HashResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetSha3MctCaseAsync(param);
            }
        }

        public async Task<MctResult<HashResult>> GetShakeMctCaseAsync(ShakeParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverShakeMctCaseGrain, MctResult<HashResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetShakeMctCaseAsync(param);
            }
        }

        public virtual async Task<MctResult<CShakeResult>> GetCShakeMctCaseAsync(CShakeParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverCShakeMctCaseGrain, MctResult<CShakeResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetCShakeMctCaseAsync(param);
            }
        }

        public virtual async Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverParallelHashMctCaseGrain, MctResult<ParallelHashResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetParallelHashMctCaseAsync(param);
            }
        }

        public virtual async Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverTupleHashMctCaseGrain, MctResult<TupleHashResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetTupleHashMctCaseAsync(param);
            }
        }
    }
}

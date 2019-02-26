using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Cshake;
using NIST.CVP.Orleans.Grains.Interfaces.Hash;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<HashResult> GetShaCaseAsync(ShaParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverShaCaseGrain, HashResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<HashResult> GetSha3CaseAsync(Sha3Parameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverSha3CaseGrain, HashResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<CShakeResult> GetCShakeCaseAsync(CShakeParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverCShakeCaseGrain, CShakeResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<ParallelHashResult> GetParallelHashCaseAsync(ParallelHashParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverParallelHashCaseGrain, ParallelHashResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TupleHashResult> GetTupleHashCaseAsync(TupleHashParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTupleHashCaseGrain, TupleHashResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public virtual async Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverShaMctCaseGrain, MctResult<HashResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public virtual async Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverSha3MctCaseGrain, MctResult<HashResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<MctResult<HashResult>> GetShakeMctCaseAsync(ShakeParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverShakeMctCaseGrain, MctResult<HashResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public virtual async Task<MctResult<CShakeResult>> GetCShakeMctCaseAsync(CShakeParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverCShakeMctCaseGrain, MctResult<CShakeResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public virtual async Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverParallelHashMctCaseGrain, MctResult<ParallelHashResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public virtual async Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTupleHashMctCaseGrain, MctResult<TupleHashResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }
    }
}

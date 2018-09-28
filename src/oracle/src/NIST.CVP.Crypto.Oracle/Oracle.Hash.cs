using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
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
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<HashResult> GetSha3CaseAsync(Sha3Parameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverSha3CaseGrain, HashResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<CShakeResult> GetCShakeCaseAsync(CShakeParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverCShakeCaseGrain, CShakeResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<ParallelHashResult> GetParallelHashCaseAsync(ParallelHashParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverParallelHashCaseGrain, ParallelHashResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TupleHashResult> GetTupleHashCaseAsync(TupleHashParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTupleHashCaseGrain, TupleHashResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<HashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.SHA_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

			var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverShaMctCaseGrain, MctResult<HashResult>>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param)
        {
            var poolBoy = new PoolBoy<MctResult<HashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.SHA3_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverSha3MctCaseGrain, MctResult<HashResult>>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<MctResult<CShakeResult>> GetCShakeMctCaseAsync(CShakeParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<CShakeResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.CSHAKE_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverCShakeMctCaseGrain, MctResult<CShakeResult>>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<ParallelHashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.PARALLEL_HASH_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverParallelHashMctCaseGrain, MctResult<ParallelHashResult>>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<TupleHashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.TUPLE_HASH_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }
            
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTupleHashMctCaseGrain, MctResult<TupleHashResult>>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }
    }
}

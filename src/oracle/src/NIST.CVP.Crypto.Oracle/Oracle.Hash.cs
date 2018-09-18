using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Cshake;
using NIST.CVP.Orleans.Grains.Interfaces.Hash;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<HashResult> GetShaCaseAsync(ShaParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverShaCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<HashResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<HashResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            return await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);
        }

        public async Task<HashResult> GetSha3CaseAsync(Sha3Parameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverSha3CaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<HashResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<HashResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<CShakeResult> GetCShakeCaseAsync(CShakeParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverCShakeCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<CShakeResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<CShakeResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<ParallelHashResult> GetParallelHashCaseAsync(ParallelHashParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverParallelHashCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<ParallelHashResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<ParallelHashResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<TupleHashResult> GetTupleHashCaseAsync(TupleHashParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverTupleHashCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<TupleHashResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<TupleHashResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<HashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.SHA_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

			var grain = _clusterClient.GetGrain<IOracleObserverShaMctCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<MctResult<HashResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<MctResult<HashResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param)
        {
            var poolBoy = new PoolBoy<MctResult<HashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.SHA3_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

			var grain = _clusterClient.GetGrain<IOracleObserverSha3MctCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<MctResult<HashResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<MctResult<HashResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<MctResult<CShakeResult>> GetCShakeMctCaseAsync(CShakeParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<CShakeResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.CSHAKE_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            var grain = _clusterClient.GetGrain<IOracleObserverCShakeMctCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<MctResult<CShakeResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<MctResult<CShakeResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<ParallelHashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.PARALLEL_HASH_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            var grain = _clusterClient.GetGrain<IOracleObserverParallelHashMctCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<MctResult<ParallelHashResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<MctResult<ParallelHashResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<TupleHashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.TUPLE_HASH_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }
            
            var grain = _clusterClient.GetGrain<IOracleObserverTupleHashMctCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<MctResult<TupleHashResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<MctResult<TupleHashResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
    }
}

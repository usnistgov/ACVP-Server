using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<AesResult> GetAesCaseAsync(AesParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesCaseGrain, AesResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }
        
        public virtual async Task<MctResult<AesResult>> GetAesMctCaseAsync(AesParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesMctCaseGrain, MctResult<AesResult>>();
            
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AesXtsResult> GetAesXtsCaseAsync(AesXtsParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesXtsCaseGrain, AesXtsResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AesResult> GetDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesDeferredCounterCaseGrain, AesResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AesResult> CompleteDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesCompleteDeferredCounterCaseGrain, AesResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<CounterResult> ExtractIvsAsync(AesParameters param, AesResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesCounterExtractIvsCaseGrain, CounterResult>();
            await observableGrain.Grain.BeginWorkAsync(param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }

        private BitString GetStartingIv(bool overflow, bool incremental)
        {
            var rand = new Random800_90();
            BitString padding;

            // Arbitrary 'small' value so samples and normal registrations always hit boundary
            //int randomBits = _isSample ? 6 : 9;
            int randomBits = 6;

            if (overflow == incremental)
            {
                padding = BitString.Ones(128 - randomBits);
            }
            else
            {
                padding = BitString.Zeroes(128 - randomBits);
            }

            return BitString.ConcatenateBits(padding, rand.GetRandomBitString(randomBits));
        }
    }
}

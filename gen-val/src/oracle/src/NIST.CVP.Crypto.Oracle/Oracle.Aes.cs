using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<AesResult> GetAesCaseAsync(AesParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesCaseGrain, AesResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
        
        public virtual async Task<MctResult<AesResult>> GetAesMctCaseAsync(AesParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesMctCaseGrain, MctResult<AesResult>>();
            
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AesXtsResult> GetAesXtsCaseAsync(AesXtsParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesXtsCaseGrain, AesXtsResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AesResult> GetAesFfCaseAsync(AesFfParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesFfCaseGrain, AesResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AesResult> GetDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesDeferredCounterCaseGrain, AesResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AesResult> CompleteDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesCompleteDeferredCounterCaseGrain, AesResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<CounterResult> ExtractIvsAsync(AesParameters param, AesResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesCounterExtractIvsCaseGrain, CounterResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AesResult> GetAesCaseAsync(AesWithPayloadParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverAesWithPayloadCaseGrain, AesResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

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

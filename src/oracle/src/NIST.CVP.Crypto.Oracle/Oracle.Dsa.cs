using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Dsa;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public virtual async Task<DsaDomainParametersResult> GetDsaPQAsync(DsaDomainParametersParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverDsaPqCaseGrain, DsaDomainParametersResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<DsaDomainParametersResult> GetDsaGAsync(DsaDomainParametersParameters param, DsaDomainParametersResult pqParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverDsaGCaseGrain, DsaDomainParametersResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, pqParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public virtual async Task<DsaDomainParametersResult> GetDsaDomainParametersAsync(DsaDomainParametersParameters param)
        {
            var pqResult = await GetDsaPQAsync(param);
            var gResult = pqResult;
			if (pqResult.G == null)
            {
                // Only try to get a G if the previous call didn't access the pool
                gResult = await GetDsaGAsync(param, pqResult);
            }

            return new DsaDomainParametersResult
            {
                Counter = pqResult.Counter,
                G = gResult.G,
                H = gResult.H,
                Index = gResult.Index,
                P = pqResult.P,
                Q = pqResult.Q,
                Seed = pqResult.Seed
            };
        }

        public async Task<VerifyResult<DsaDomainParametersResult>> GetDsaPQVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverDsaPqVerifyCaseGrain, VerifyResult<DsaDomainParametersResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<DsaDomainParametersResult>> GetDsaGVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverDsaGVerifyCaseGrain, VerifyResult<DsaDomainParametersResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<DsaKeyResult> GetDsaKeyAsync(DsaKeyParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverDsaKeyCaseGrain, DsaKeyResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<DsaKeyResult>> CompleteDeferredDsaKeyAsync(DsaKeyParameters param, DsaKeyResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverDsaCompleteDeferredKeyCaseGrain, VerifyResult<DsaKeyResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<DsaSignatureResult> GetDeferredDsaSignatureAsync(DsaSignatureParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverDsaDeferredSignatureCaseGrain, DsaSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<DsaSignatureResult>> CompleteDeferredDsaSignatureAsync(DsaSignatureParameters param, DsaSignatureResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverDsaCompleteDeferredSignatureCaseGrain, VerifyResult<DsaSignatureResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<DsaSignatureResult> GetDsaSignatureAsync(DsaSignatureParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverDsaSignatureCaseGrain, DsaSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
    }
}

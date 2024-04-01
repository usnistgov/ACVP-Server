using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Exceptions;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public partial class Oracle
    {
        public virtual async Task<LmsKeyPairResult> GetLmsKeyCaseAsync(LmsKeyPairParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverLmsKeyCaseGrain, LmsKeyPairResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<LmsSignatureResult> GetDeferredLmsSignatureCaseAsync(LmsSignatureParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverLmsDeferredSignatureCaseGrain, LmsSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<LmsSignatureResult> GetLmsSignatureCaseAsync(LmsSignatureParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverLmsSignatureCaseGrain, LmsSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<LmsVerificationResult> CompleteDeferredLmsSignatureAsync(LmsSignatureParameters param, LmsSignatureResult providedResult)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverLmsCompleteDeferredSignatureCaseGrain, LmsVerificationResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, providedResult, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredLmsSignatureAsync(param, providedResult);
            }
        }
        
        public async Task<VerifyResult<LmsSignatureResult>> GetLmsVerifyResultAsync(LmsSignatureParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverLmsVerifyCaseGrain, VerifyResult<LmsSignatureResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
    }
}

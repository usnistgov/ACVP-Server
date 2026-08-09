using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Exceptions;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xmss;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public partial class Oracle
    {
        public virtual async Task<XmssKeyPairResult> GetXmssKeyCaseAsync(XmssKeyPairParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverXmssKeyCaseGrain, XmssKeyPairResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<XmssSignatureResult> GetDeferredXmssSignatureCaseAsync(XmssSignatureParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverXmssDeferredSignatureCaseGrain, XmssSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<XmssSignatureResult> GetXmssSignatureCaseAsync(XmssSignatureParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverXmssSignatureCaseGrain, XmssSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<XmssVerificationResult> CompleteDeferredXmssSignatureAsync(XmssSignatureParameters param, XmssSignatureResult providedResult)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverXmssCompleteDeferredSignatureCaseGrain, XmssVerificationResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, providedResult, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredXmssSignatureAsync(param, providedResult);
            }
        }

        public async Task<VerifyResult<XmssSignatureResult>> GetXmssVerifyResultAsync(XmssSignatureParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverXmssVerifyCaseGrain, VerifyResult<XmssSignatureResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
    }
}

using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<LmsKeyResult> GetLmsKeyCaseAsync(LmsKeyParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverLmsKeyCaseGrain, LmsKeyResult>();
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

        public async Task<VerifyResult<LmsSignatureResult>> GetLmsVerifyResultAsync(LmsSignatureParameters param)
        {
            var keyParam = new LmsKeyParameters
            {
                Layers = param.Layers,
                LmotsTypes = param.LmotsTypes,
                LmsTypes = param.LmsTypes
            };

            var key = await GetLmsKeyCaseAsync(keyParam);
            // re-signs with "bad key" under specific error condition to ensure IUT validates as failed verification.
            LmsKeyResult badKey = null;
            if (param.Disposition == LmsSignatureDisposition.ModifyKey)
            {
                badKey = await GetLmsKeyCaseAsync(keyParam);
            }

            var observableGrain =
                await GetObserverGrain<IOracleObserverLmsVerifySignatureCaseGrain, VerifyResult<LmsSignatureResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, key, badKey, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<MctResult<LmsSignatureResult>> GetLmsMctCaseAsync(LmsSignatureParameters param)
        {
            var observableGrain =
                await GetObserverGrain<IOracleObserverLmsMctCaseGrain, MctResult<LmsSignatureResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
    }
}

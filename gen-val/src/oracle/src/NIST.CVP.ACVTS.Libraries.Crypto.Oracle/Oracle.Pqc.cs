using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle;

public partial class Oracle
{
    public async Task<MLDSAKeyPairResult> GetMLDSAKeyCaseAsync(MLDSAKeyGenParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLDSAKeyCaseGrain, MLDSAKeyPairResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }

    public async Task<MLDSASignatureResult> GetMLDSASigGenCaseAsync(MLDSASignatureParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLDSASignatureCaseGrain, MLDSASignatureResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }
    
    public async Task<VerifyResult<MLDSASignatureResult>> GetMLDSAVerifyResultAsync(MLDSASignatureParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLDSAVerifyCaseGrain, VerifyResult<MLDSASignatureResult>>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }
    
    public async Task<MLKEMKeyPairResult> GetMLKEMKeyCaseAsync(MLKEMKeyGenParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLKEMKeyCaseGrain, MLKEMKeyPairResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }

    public async Task<MLKEMEncapsulationResult> GetMLKEMEncapCaseAsync(MLKEMEncapsulationParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLKEMEncapCaseGrain, MLKEMEncapsulationResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }
    
    public async Task<MLKEMEncapsulationResult> GetMLKEMEncapDeferredCaseAsync(MLKEMEncapsulationParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLKEMEncapCompleteDeferredCaseGrain, MLKEMEncapsulationResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }
    
    public async Task<MLKEMEncapsulationResult> GetMLKEMDecapCaseAsync(MLKEMDecapsulationParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLKEMDecapCaseGrain, MLKEMEncapsulationResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }

    public async Task<MLKEMDecapsulationResult> CompleteDeferredMLKEMEncapsulationAsync(MLKEMDecapsulationParameters param, MLKEMEncapsulationResult providedResult)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLKEMDeferredEncapCaseGrain, MLKEMDecapsulationResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, providedResult, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }
}

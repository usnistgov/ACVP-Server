using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle;

public partial class Oracle
{
    // ML-DSA KeyGen
    public async Task<MLDSAKeyPairResult> GetMLDSAKeyCaseAsync(MLDSAKeyGenParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLDSAKeyCaseGrain, MLDSAKeyPairResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }

    // ML-DSA SigGen
    public async Task<MLDSASignatureResult> GetMLDSASigGenCaseAsync(MLDSASignatureParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLDSASignatureCaseGrain, MLDSASignatureResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }
    
    public virtual Task<MLDSASignatureResult> GetMLDSASigGenCornerCaseAsync(MLDSASignatureParameters param)
    {
        throw new NotImplementedException("Only available with pools");
    }

    public async Task<MLDSASignatureResult> CompleteMLDSASigGenCornerCaseAsync(MLDSASignatureParameters param, MLDSASignatureResult poolResult)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLDSACompleteSignatureCornerCaseGrain, MLDSASignatureResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, poolResult, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }
    
    // ML-DSA SigVer
    public async Task<VerifyResult<MLDSASignatureResult>> GetMLDSAVerifyResultAsync(MLDSASignatureParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLDSAVerifyCaseGrain, VerifyResult<MLDSASignatureResult>>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }
    
    // ML-KEM KeyGen
    public async Task<MLKEMKeyPairResult> GetMLKEMKeyCaseAsync(MLKEMKeyGenParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLKEMKeyCaseGrain, MLKEMKeyPairResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }

    // ML-KEM EncapDecap
    public async Task<MLKEMEncapsulationResult> GetMLKEMEncapCaseAsync(MLKEMEncapsulationParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLKEMEncapCaseGrain, MLKEMEncapsulationResult>();
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

    public async Task<MLKEMKeyPairResult> GetMLKEMEncapKeyCheckCaseAsync(MLKEMKeyGenParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLKEMEncapKeyCheckCaseGrain, MLKEMKeyPairResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }

    public async Task<MLKEMKeyPairResult> GetMLKEMDecapKeyCheckCaseAsync(MLKEMKeyGenParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverMLKEMDecapKeyCheckCaseGrain, MLKEMKeyPairResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }

    // SLH-DSA KeyGen
    public async Task<SLHDSAKeyPairResult> GetSLHDSAKeyCaseAsync(SLHDSAKeyGenParameters param)
    {
        var observableGrain = await GetObserverGrain<IOracleObserverSLHDSAKeyCaseGrain, SLHDSAKeyPairResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }

    // SLH-DSA SigGen
    public async Task<SLHDSASignatureResult> GetSLHDSASigGenCaseAsync(SLHDSASignatureParameters param)
    {
        var observableGrain = await GetObserverGrain<IOracleObserverSLHDSASignatureCaseGrain, SLHDSASignatureResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }
    
    // SLH-DSA SigVer
    public async Task<VerifyResult<SLHDSASignatureResult>> GetSLHDSASigVerCaseAsync(SLHDSASignatureParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverSLHDSASignatureVerifyCaseGrain, VerifyResult<SLHDSASignatureResult>>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }
}

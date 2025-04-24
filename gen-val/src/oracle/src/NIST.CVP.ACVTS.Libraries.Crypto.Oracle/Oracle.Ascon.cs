using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Ascon;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Ascon;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ascon;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle;

public partial class Oracle
{
    public async Task<AsconAead128Result> GetAsconEncryptCaseAsync(AsconAEAD128Parameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverAsconEncrypt128CaseGrain, AsconAead128Result>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }

    public async Task<AsconAead128Result> GetAsconDecryptCaseAsync(AsconAEAD128Parameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverAsconDecrypt128CaseGrain, AsconAead128Result>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }

    public async Task<AsconHashResult> GetAsconHash256CaseAsync(AsconHashParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverAsconHash256CaseGrain, AsconHashResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }

    public async Task<AsconHashResult> GetAsconXOF128CaseAsync(AsconHashParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverAsconXOF128CaseGrain, AsconHashResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }

    public async Task<AsconHashResult> GetAsconCXOF128CaseAsync(AsconHashParameters param)
    {
        var observableGrain =
            await GetObserverGrain<IOracleObserverAsconCXOF128CaseGrain, AsconHashResult>();
        await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

        return await observableGrain.ObserveUntilResult();
    }
}

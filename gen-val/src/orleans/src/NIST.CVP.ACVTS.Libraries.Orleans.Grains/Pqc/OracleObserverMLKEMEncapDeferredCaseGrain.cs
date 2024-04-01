using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Kyber;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverMLKEMEncapDeferredCaseGrain : ObservableOracleGrainBase<MLKEMEncapsulationResult>,
    IOracleObserverMLKEMEncapCaseGrain
{
    private MLKEMEncapsulationParameters _param;
    private IRandom800_90 _rand;

    public OracleObserverMLKEMEncapDeferredCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IRandom800_90 rand
    ) : base(nonOrleansScheduler)
    {
        _rand = rand;
    }

    public async Task<bool> BeginWorkAsync(MLKEMEncapsulationParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        // Generate a random seed used in encapsulation and nothing else
        await Notify(new MLKEMEncapsulationResult { SeedM = _rand.GetRandomBitString(256) });
    }
}

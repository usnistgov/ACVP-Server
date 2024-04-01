using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverMLDSADeferredSignatureCaseGrain : ObservableOracleGrainBase<MLDSASignatureResult>, IOracleObserverMLDSADeferredSignatureCaseGrain
{
    private MLDSASignatureParameters _param;
    private readonly IRandom800_90 _random;

    public OracleObserverMLDSADeferredSignatureCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IRandom800_90 random
    ) : base(nonOrleansScheduler)
    {
        _random = random;
    }

    public async Task<bool> BeginWorkAsync(MLDSASignatureParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var message = _random.GetRandomBitString(_param.MessageLength);

        // Notify observers of result
        await Notify(new MLDSASignatureResult
        {
            Message = message
        });
    }
}

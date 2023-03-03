using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Lms;

public class OracleObserverLmsDeferredSignatureCaseGrain : ObservableOracleGrainBase<LmsSignatureResult>, IOracleObserverLmsDeferredSignatureCaseGrain
{
    private readonly IEntropyProvider _entropyProvider;
    private LmsSignatureParameters _param;

    public OracleObserverLmsDeferredSignatureCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IEntropyProviderFactory entropyProviderFactory
    ) : base(nonOrleansScheduler)
    {
        _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
    }

    public async Task<bool> BeginWorkAsync(LmsSignatureParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var message = _entropyProvider.GetEntropy(_param.MessageLength);

        // Notify observers of result
        await Notify(new LmsSignatureResult
        {
            Message = message
        });
    }
}

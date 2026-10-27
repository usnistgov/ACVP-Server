using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xmss;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xmss;

public class OracleObserverXmssDeferredSignatureCaseGrain : ObservableOracleGrainBase<XmssSignatureResult>, IOracleObserverXmssDeferredSignatureCaseGrain
{
    private readonly IEntropyProvider _entropyProvider;
    private XmssSignatureParameters _param;

    public OracleObserverXmssDeferredSignatureCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IEntropyProviderFactory entropyProviderFactory
    ) : base(nonOrleansScheduler)
    {
        _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
    }

    public async Task<bool> BeginWorkAsync(XmssSignatureParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var message = _entropyProvider.GetEntropy(_param.MessageLength);

        // Notify observers of result
        await Notify(new XmssSignatureResult
        {
            Message = message
        });
    }
}

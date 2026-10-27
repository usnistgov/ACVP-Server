using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xmss;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xmss;

public class OracleObserverXmssCompleteDeferredSignatureCaseGrain : ObservableOracleGrainBase<XmssVerificationResult>, IOracleObserverXmssCompleteDeferredSignatureCaseGrain
{
    private readonly IXmssVerifier _xmssVerifier;

    private XmssSignatureParameters _param;
    private XmssSignatureResult _providedResult;

    public OracleObserverXmssCompleteDeferredSignatureCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IXmssVerifier xmssVerifier
    ) : base(nonOrleansScheduler)
    {
        _xmssVerifier = xmssVerifier;
    }

    public async Task<bool> BeginWorkAsync(XmssSignatureParameters param, XmssSignatureResult providedResult)
    {
        _param = param;
        _providedResult = providedResult;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var result = _xmssVerifier.Verify(_providedResult.PublicKey.ToBytes(), _providedResult.Signature.ToBytes(), _providedResult.Message.ToBytes());

        // Notify observers of result
        await Notify(result);
    }
}

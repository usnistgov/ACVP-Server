using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xmss;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xmss;

public class OracleObserverXmssSignatureCaseGrain : ObservableOracleGrainBase<XmssSignatureResult>, IOracleObserverXmssSignatureCaseGrain
{
    private readonly IRandom800_90 _random;
    private readonly IXmssSigner _xmssSigner;
    private XmssSignatureParameters _param;

    public OracleObserverXmssSignatureCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IRandom800_90 random,
        IXmssSigner xmssSigner
    ) : base(nonOrleansScheduler)
    {
        _random = random;
        _xmssSigner = xmssSigner;
    }

    public async Task<bool> BeginWorkAsync(XmssSignatureParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var message = _random.GetRandomBitString(_param.MessageLength);

        // We don't actually track the state of the tree, so use the idx the gen/vals tells us, which should be unique
        _param.XmssKeyPair.PrivateKey.SetIdx(_param.Idx);

        // Sign the message
        var result = _xmssSigner.Sign(_param.XmssKeyPair.PrivateKey, message.ToBytes());

        // Notify observers of result
        await Notify(new XmssSignatureResult
        {
            Message = message,
            Signature = new BitString(result.Signature)
        });
    }
}

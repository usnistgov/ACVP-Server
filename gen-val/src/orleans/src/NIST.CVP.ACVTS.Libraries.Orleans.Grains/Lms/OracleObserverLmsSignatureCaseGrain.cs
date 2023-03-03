using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Lms;

public class OracleObserverLmsSignatureCaseGrain : ObservableOracleGrainBase<LmsSignatureResult>, IOracleObserverLmsSignatureCaseGrain
{
    private readonly IRandom800_90 _random;
    private readonly ILmsSigner _lmsSigner;
    private readonly ILmOtsRandomizerC _randomizerC;
    private LmsSignatureParameters _param;

    public OracleObserverLmsSignatureCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IRandom800_90 random,
        ILmsSigner lmsSigner,
        ILmOtsRandomizerC randomizerC
    ) : base(nonOrleansScheduler)
    {
        _random = random;
        _lmsSigner = lmsSigner;
        _randomizerC = randomizerC;
    }

    public async Task<bool> BeginWorkAsync(LmsSignatureParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var message = _random.GetRandomBitString(_param.MessageLength);

        // We don't actually track the state of the tree so pick a random Q in the tree. This is done for every individual test case, so there is a slight chance a Q is duplicated in samples
        var leafCount = 1 << _param.LmsKeyPair.LmsAttribute.H;
        _param.LmsKeyPair.PrivateKey.SetQ(_random.GetRandomInt(0, leafCount));
        
        // Sign the message
        var result = _lmsSigner.Sign(_param.LmsKeyPair.PrivateKey, _randomizerC, message.ToBytes());
        
        // Notify observers of result
        await Notify(new LmsSignatureResult
        {
            Message = message,
            Signature = new BitString(result.Signature)
        });
    }
}

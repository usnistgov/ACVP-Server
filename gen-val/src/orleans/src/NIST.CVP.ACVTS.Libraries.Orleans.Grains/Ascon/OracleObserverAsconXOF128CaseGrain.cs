using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ascon;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Ascon;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Ascon;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Common;
using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Ascon;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Ascon;

public class OracleObserverAsconXOF128CaseGrain : ObservableOracleGrainBase<AsconHashResult>, IOracleObserverAsconXOF128CaseGrain
{
    private AsconHashParameters _param;
    private readonly IRandom800_90 _rand;
    private Crypto.Ascon.Ascon ascon = new Crypto.Ascon.Ascon();

    public OracleObserverAsconXOF128CaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler, IRandom800_90 rand) : base(nonOrleansScheduler)
    {
        _rand = rand;
    }

    public async Task<bool> BeginWorkAsync(AsconHashParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var message = _rand.GetRandomBitString(_param.MessageBitLength).ToBytes().Reverse().ToArray();
        var result = new AsconHashResult();

        var asconResult = ascon.Xof128(message, _param.MessageBitLength, _param.DigestBitLength);

        result.Digest = new BitString(asconResult);
        result.Message = new BitString(message);

        await Notify(result);
    }
}

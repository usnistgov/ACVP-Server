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

public class OracleObserverAsconEncrypt128CaseGrain : ObservableOracleGrainBase<AsconAead128Result>, IOracleObserverAsconEncrypt128CaseGrain
{
    private AsconAEAD128Parameters _param;
    private readonly IRandom800_90 _rand;
    private Crypto.Ascon.Ascon ascon = new Crypto.Ascon.Ascon();

    public OracleObserverAsconEncrypt128CaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler, IRandom800_90 rand) : base(nonOrleansScheduler)
    {
        _rand = rand;
    }

    public async Task<bool> BeginWorkAsync(AsconAEAD128Parameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var key = _rand.GetRandomBitString(128).ToBytes();
        var nonce = _rand.GetRandomBitString(128).ToBytes();
        var ad = _rand.GetRandomBitString(_param.ADBitLength).ToBytes().Reverse().ToArray();
        var plaintext = _rand.GetRandomBitString(_param.PayloadBitLength).ToBytes().Reverse().ToArray();
        var result = new AsconAead128Result();

        (byte[] c, byte[] tag) asconResult;
        if (_param.NonceMasking)
        {
            var secondKey = _rand.GetRandomBitString(128).ToBytes();
            asconResult = ascon.Aead128Encrypt(key, nonce, ad, plaintext, _param.PayloadBitLength, _param.ADBitLength, _param.TruncationLength, secondKey);
            result.SecondKey = new BitString(secondKey);
        }
        else
        {
            asconResult = ascon.Aead128Encrypt(key, nonce, ad, plaintext, _param.PayloadBitLength, _param.ADBitLength, _param.TruncationLength);
        }

        result.Ciphertext = new BitString(asconResult.c);
        result.Tag = new BitString(asconResult.tag);
        result.Key = new BitString(key);
        result.Nonce = new BitString(nonce);
        result.AD = new BitString(ad);
        result.Plaintext = new BitString(plaintext);

        await Notify(result);
    }
}

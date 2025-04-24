using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ascon;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Ascon;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Ascon;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Common;
using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Ascon;
using System.Linq;
using System.Collections.Immutable;
using System.Threading;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Ascon;

public class OracleObserverAsconDecrypt128CaseGrain : ObservableOracleGrainBase<AsconAead128Result>, IOracleObserverAsconDecrypt128CaseGrain
{
    private AsconAEAD128Parameters _param;
    private readonly IRandom800_90 _rand;

    public OracleObserverAsconDecrypt128CaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler, IRandom800_90 rand) : base(nonOrleansScheduler)
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

        var ascon = new Crypto.Ascon.Ascon();

        (byte[] c, byte[] tag) encryptResult;
        AsconDecryptResult decryptResult;
        if (_param.NonceMasking)
        {
            var secondKey = _rand.GetRandomBitString(128).ToBytes();
            //Create copy of nonce since it is modified by encrypt
            var noncecopy = new byte[nonce.Length];
            nonce.CopyTo(noncecopy, 0);
            encryptResult = ascon.Aead128Encrypt(key, nonce, ad, plaintext, _param.PayloadBitLength, _param.ADBitLength, _param.TruncationLength, secondKey);
            decryptResult = ascon.Aead128Decrypt(key, noncecopy, ad, encryptResult.c, encryptResult.tag, _param.PayloadBitLength, _param.ADBitLength, _param.TruncationLength, secondKey);
            result.SecondKey = new BitString(secondKey);
        }
        else
        {
            encryptResult = ascon.Aead128Encrypt(key, nonce, ad, plaintext, _param.PayloadBitLength, _param.ADBitLength, _param.TruncationLength);
            decryptResult =  ascon.Aead128Decrypt(key, nonce, ad, encryptResult.c, encryptResult.tag, _param.PayloadBitLength, _param.ADBitLength, _param.TruncationLength);
        }

        result.Ciphertext = new BitString(encryptResult.c);
        result.Tag = new BitString(encryptResult.tag);
        result.Key = new BitString(key);
        result.Nonce = new BitString(nonce);
        result.AD = new BitString(ad);
        if (decryptResult.HasResult)
        {
            result.Plaintext = new BitString(decryptResult.Result);
        }
        else
        {
            throw new Exception("Ascon AEAD decrypt failure");
        }
        

        await Notify(result);
    }
}

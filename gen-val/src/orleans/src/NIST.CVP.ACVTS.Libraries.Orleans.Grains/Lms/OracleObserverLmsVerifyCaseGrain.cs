using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms;
using LmsSignatureResult = NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.LmsSignatureResult;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Lms;

public class OracleObserverLmsVerifyCaseGrain : ObservableOracleGrainBase<VerifyResult<LmsSignatureResult>>, IOracleObserverLmsVerifyCaseGrain
{
    private LmsSignatureParameters _param;
    private readonly IRandom800_90 _rand;
    private readonly ILmsSigner _lmsSigner;
    private readonly ILmOtsRandomizerC _randomizer;
    
    public OracleObserverLmsVerifyCaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        ILmsSigner lmsSigner,
        ILmOtsRandomizerC randomizer,
        IRandom800_90 rand) : base(nonOrleansScheduler)
    {
        _rand = rand;
        _lmsSigner = lmsSigner;
        _randomizer = randomizer;
    }

    public async Task<bool> BeginWorkAsync(LmsSignatureParameters param)
    {
        _param = param;
        
        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }
    
    protected override async Task DoWorkAsync()
    {
        var message = _rand.GetRandomBitString(_param.MessageLength);
        
        // We don't actually track the state of the tree so pick a random Q in the tree. This is done for every individual test case, so there is a slight chance a Q is duplicated in samples
        var leafCount = 1 << _param.LmsKeyPair.LmsAttribute.H;
        _param.LmsKeyPair.PrivateKey.SetQ(_rand.GetRandomInt(0, leafCount));
        
        var signature = _lmsSigner.Sign(_param.LmsKeyPair.PrivateKey, _randomizer, message.ToBytes());

        var result = new LmsSignatureResult
        {
            Message = message,
            PublicKey = new BitString(_param.LmsKeyPair.PublicKey.Key),
            Signature = new BitString(signature.Signature)
        };

        switch (_param.Disposition)
        {
            case LmsSignatureDisposition.None:
                break;
            
            case LmsSignatureDisposition.ModifyHeader:
                // Pick a random other LmsAttribute and use that as the leading 4 bytes of the key
                var allLmsModes = EnumHelpers.GetEnums<LmsMode>();
                LmsMode newLmsMode;
                do
                {
                    newLmsMode = allLmsModes[_rand.GetRandomInt(0, allLmsModes.Count)];
                } while (newLmsMode == _param.LmsMode || newLmsMode == LmsMode.Invalid);

                var newLmsAttribute = AttributesHelper.GetLmsAttribute(newLmsMode);
                var keyLength = result.PublicKey.BitLength / 8;
                var newPublicKey = new byte[keyLength];
                Array.Copy(newLmsAttribute.NumericIdentifier, 0, newPublicKey, 0, 4);
                Array.Copy(result.PublicKey.ToBytes(), 4, newPublicKey, 4, keyLength - 4);
                
                break;
            
            case LmsSignatureDisposition.ModifyKey:
                // Flip the last bit in the key
                var lastBitKeyIndex = result.PublicKey.BitLength - 1;
                result.PublicKey.Bits.Set(lastBitKeyIndex, !result.PublicKey.Bits.Get(lastBitKeyIndex));
                break;
            
            case LmsSignatureDisposition.ModifyMessage:
                // Flip the last bit in the message
                var lastBitMessageIndex = result.Message.BitLength - 1;
                result.Message.Bits.Set(lastBitMessageIndex, !result.Message.Bits.Get(lastBitMessageIndex));
                break;
            
            case LmsSignatureDisposition.ModifySignature:
                // Flip the last bit in the signature
                var lastBitSignatureIndex = result.Signature.BitLength - 1;
                result.Signature.Bits.Set(lastBitSignatureIndex, !result.Signature.Bits.Get(lastBitSignatureIndex));
                break;
        }
        
        // Notify observers of result
        await Notify(new VerifyResult<LmsSignatureResult>
        {
            Result = _param.Disposition == LmsSignatureDisposition.None,
            VerifiedValue = result
        });
    }
}

using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverSLHDSASignatureVerifyCaseGrain : ObservableOracleGrainBase<VerifyResult<SLHDSASignatureResult>>, IOracleObserverSLHDSASignatureVerifyCaseGrain
{
    private readonly IRandom800_90 _rand;
    private readonly IShaFactory _shaFactory;

    private SLHDSASignatureParameters _param;

    public OracleObserverSLHDSASignatureVerifyCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IShaFactory shaFactory,
        IRandom800_90 rand
    ) : base(nonOrleansScheduler)
    {
        _rand = rand;
        _shaFactory = shaFactory;
    }
    
    public async Task<bool> BeginWorkAsync(SLHDSASignatureParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        // the message to be signed
        var message = _rand.GetRandomBitString(_param.MessageLength);
        
        BitString context = null;    
        if (_param.SignatureInterface == SignatureInterface.External)
        {
            context = _rand.GetRandomBitString(_param.ContextLength);
        }
        
        // set up slh-dsa parameters object
        var slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(_param.SlhdsaParameterSet);
        
        // for internal, we can use the random value directly
        // for external, we need to pass it through an entropy provider
        // we can set up both at the same time for all cases
        var additionalRandomness = _param.Deterministic ? new BitString(0) : _rand.GetRandomBitString(slhdsaParameterSetAttributes.N * 8);
        var testableEntropy = new TestableEntropyProvider(true);
        testableEntropy.AddEntropy(additionalRandomness);
        
        var slhdsa = new Slhdsa(slhdsaParameterSetAttributes, _shaFactory, testableEntropy);

        // determine which signature path to use based on the test case/group parameters
        var signature = (_param.SignatureInterface, _param.PreHash) switch
        {
            (SignatureInterface.Internal, _) => slhdsa.Sign(_param.PrivateKey.ToBytes(), message.ToBytes(), additionalRandomness.ToBytes()),
            
            (SignatureInterface.External, PreHash.Pure) => slhdsa.ExternalSign(_param.PrivateKey.ToBytes(), message.ToBytes(), _param.Deterministic, context.ToBytes()),
            (SignatureInterface.External, PreHash.PreHash) => slhdsa.ExternalPreHashSign(_param.PrivateKey.ToBytes(), message.ToBytes(), _param.Deterministic, context.ToBytes(), ShaAttributes.GetHashFunctionFromEnum(_param.HashFunction)),
            
            (_, _) => throw new ArgumentException("Invalid combination of parameters for SLH-DSA")
        };
        
        var result = new SLHDSASignatureResult
        {
            AdditionalRandomness = additionalRandomness,
            MessageLength = _param.MessageLength,
            Message = message,
            Signature = new BitString(signature),
            Context = context
        };

        // Make any changes that may be required based on the test case disposition
        // SLH-DSA Signature format:
        // Randomness R - n bytes
        // SIGFORS - k(1 + a) * n bytes
        // SIGHT - (h + d * len) * n bytes
        switch (_param.Disposition)
        {
            case SLHDSASignatureDisposition.None:
                // no changes required
                break;
            
            case SLHDSASignatureDisposition.ModifyMessage:
                // Flip the last bit in the message, Bits is Lsb
                result.Message.Bits.Set(0, !result.Message.Bits.Get(0));
                break;
            
            case SLHDSASignatureDisposition.ModifySignatureR:
                // Flip the first bit in the signature, Bits is Lsb
                var leadingBit = result.Signature.Bits.Count - 1;
                result.Signature.Bits.Set(leadingBit, !result.Signature.Bits.Get(leadingBit));
                break;
            
            case SLHDSASignatureDisposition.ModifySignatureSigFors:
                // Flip the first bit in the SIGFORS component of the signature, Bits is Lsb
                var leadingSigForsBit = result.Signature.Bits.Count - 1 - (slhdsaParameterSetAttributes.N * 8);
                result.Signature.Bits.Set(leadingSigForsBit, !result.Signature.Bits.Get(leadingSigForsBit));
                break;
            
            case SLHDSASignatureDisposition.ModifySignatureSigHt:
                // Flip the last bit in the SIGHT component of the signature, Bits is Lsb
                result.Signature.Bits.Set(0, !result.Signature.Bits.Get(0));
                break;
            
            case SLHDSASignatureDisposition.ModifySignatureTooLarge:
                // Make the signature larger by 1 byte
                var alteredSignature = result.Signature.ToHex() + "AA";
                result.Signature = new BitString(alteredSignature);
                break;
            
            case SLHDSASignatureDisposition.ModifySignatureTooSmall:
                // Make the signature smaller by 1 byte
                var signatureString = result.Signature.ToHex();
                result.Signature = new BitString(signatureString.Substring(0, signatureString.Length - 2));
                break;
        }
        
        await Notify(new VerifyResult<SLHDSASignatureResult>
        {
            Result = _param.Disposition == SLHDSASignatureDisposition.None,
            VerifiedValue = result
        });
    }
}

using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Dilithium;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverMLDSAVerifyCaseGrain : ObservableOracleGrainBase<VerifyResult<MLDSASignatureResult>>, IOracleObserverMLDSAVerifyCaseGrain
{
    private MLDSASignatureParameters _param;

    private readonly IShaFactory _shaFactory;
    private readonly IRandom800_90 _rand;
    
    public OracleObserverMLDSAVerifyCaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IShaFactory shaFactory,
        IRandom800_90 random
    ) : base(nonOrleansScheduler)
    {
        _shaFactory = shaFactory;
        _rand = random;
    }

    public async Task<bool> BeginWorkAsync(MLDSASignatureParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var message = _rand.GetRandomBitString(_param.MessageLength);
        var mu = _rand.GetRandomBitString(512);

        BitString context = null;    
        if (_param.SignatureInterface == SignatureInterface.External)
        {
            context = _rand.GetRandomBitString(_param.ContextLength);
        }
        
        var mldsa = new Dilithium(_param.ParameterSet, _shaFactory);
        
        // Deterministic makes no difference on signature verification steps
        var signature = (_param.SignatureInterface, _param.PreHash, _param.ExternalMu) switch
        {
            (SignatureInterface.Internal, _, true) => mldsa.SignExternalMu(_param.PrivateKey.ToBytes(), mu.ToBytes(), BitString.Zeroes(256).ToBytes()),
            (SignatureInterface.Internal, _, false) => mldsa.Sign(_param.PrivateKey.ToBytes(), message.ToBytes(), BitString.Zeroes(256).ToBytes()),
            
            (SignatureInterface.External, PreHash.Pure, _) => mldsa.ExternalSign(_param.PrivateKey.ToBytes(), message.ToBytes(), _param.Deterministic, context.ToBytes()),
            (SignatureInterface.External, PreHash.PreHash, _) => mldsa.ExternalPreHashSign(_param.PrivateKey.ToBytes(), message.ToBytes(), _param.Deterministic, context.ToBytes(), ShaAttributes.GetHashFunctionFromEnum(_param.HashFunction)),
            
            (_, _, _) => throw new ArgumentException("Invalid combination of parameters for ML-DSA")
        };
        
        var result = new MLDSASignatureResult
        {
            Message = _param.ExternalMu ? null : message,
            Mu = _param.ExternalMu ? mu : null,
            Context = _param.SignatureInterface == SignatureInterface.External ? context : null,    // Only used for external
            Signature = new BitString(signature)
        };

        // Apply modifications if needed
        // First lambda/4 bits are commitment, 
        // Middle l * 32 * (1 + bitlen(gamma1 - 1)) bytes are z
        // Last omega + k bytes are hint
        switch (_param.Disposition)
        {
            case MLDSASignatureDisposition.None:
                break;
            
            case MLDSASignatureDisposition.ModifyMessage:
                // Flip the last bit in the message and mu (only one will be used), Bits is Lsb
                if (_param.ExternalMu)
                {
                    result.Mu.Bits.Set(0, !result.Mu.Bits.Get(0));
                }
                else
                {
                    result.Message.Bits.Set(0, !result.Message.Bits.Get(0));
                }
                break;
            
            case MLDSASignatureDisposition.ModifySignature:
                // Flip the first bit in the signature, Bits is Lsb
                var leadingBit = result.Signature.Bits.Count - 1;
                result.Signature.Bits.Set(leadingBit, !result.Signature.Bits.Get(leadingBit));
                break;
            
            case MLDSASignatureDisposition.ModifyHint:
                // Flip the last bit in the signature
                // TODO make sure this leads to a large hint that fails the check
                result.Signature.Bits.Set(0, !result.Signature.Bits.Get(0));
                break;
            
            case MLDSASignatureDisposition.ModifyZ:
                // Flip a bit in the middle
                // TODO make sure this leads to a large z that fails the check
                var mldsaParameters = new DilithiumParameters(_param.ParameterSet);
                var zBit = (mldsaParameters.Lambda * 2) + 1;
                result.Signature.Bits.Set(zBit, !result.Signature.Bits.Get(zBit));
                break;
        }
        
        await Notify(new VerifyResult<MLDSASignatureResult>
        {
            Result = _param.Disposition == MLDSASignatureDisposition.None,
            VerifiedValue = result
        });
    }
}

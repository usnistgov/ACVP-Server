using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Dilithium;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverMLDSASignatureCaseGrain : ObservableOracleGrainBase<MLDSASignatureResult>, IOracleObserverMLDSASignatureCaseGrain
{
    private MLDSASignatureParameters _param;

    private readonly IRandom800_90 _rand;
    private readonly IShaFactory _shaFactory;
    
    public OracleObserverMLDSASignatureCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IShaFactory shaFactory,
        IRandom800_90 rand
    ) : base(nonOrleansScheduler)
    {
        _shaFactory = shaFactory;
        _rand = rand;
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

        // Need testable entropy for the combination of deterministic = false and external
        var testableEntropy = new TestableEntropyProvider(true);
        var seed = _param.Deterministic ? BitString.Zeroes(256) : _rand.GetRandomBitString(256);
        testableEntropy.AddEntropy(seed);

        var mldsa = new Dilithium(_param.ParameterSet, _shaFactory, testableEntropy);
        
        var result = (_param.SignatureInterface, _param.PreHash, _param.ExternalMu) switch
        {
            (SignatureInterface.Internal, _, true) => mldsa.SignExternalMu(_param.PrivateKey.ToBytes(), mu.ToBytes(), seed.ToBytes()),
            (SignatureInterface.Internal, _, false) => mldsa.Sign(_param.PrivateKey.ToBytes(), message.ToBytes(), seed.ToBytes()),
            
            (SignatureInterface.External, PreHash.Pure, _) => mldsa.ExternalSign(_param.PrivateKey.ToBytes(), message.ToBytes(), _param.Deterministic, context.ToBytes()),
            (SignatureInterface.External, PreHash.PreHash, _) => mldsa.ExternalPreHashSign(_param.PrivateKey.ToBytes(), message.ToBytes(), _param.Deterministic, context.ToBytes(), ShaAttributes.GetHashFunctionFromEnum(_param.HashFunction)),
            
            (_, _, _) => throw new ArgumentException("Invalid combination of parameters for ML-DSA")
        };

        await Notify(new MLDSASignatureResult
        {
            Message = _param.ExternalMu ? null : message,
            Mu = _param.ExternalMu ? mu : null,
            Signature = new BitString(result),
            Context = _param.SignatureInterface == SignatureInterface.External ? context : null,    // Only used for external
            Rnd = !_param.Deterministic ? seed : null      // Only used for non-deterministic
        });
    }
}

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
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverSLHDSASignatureCaseGrain :  ObservableOracleGrainBase<SLHDSASignatureResult>, IOracleObserverSLHDSASignatureCaseGrain
{
    private readonly IRandom800_90 _rand;
    private readonly IShaFactory _shaFactory;

    private SLHDSASignatureParameters _param;

    public OracleObserverSLHDSASignatureCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IShaFactory shaFactory,
        IRandom800_90 rand
    ) : base(nonOrleansScheduler)
    {
        _shaFactory = shaFactory;
        _rand = rand;
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
        
        await Notify(new SLHDSASignatureResult
        {
            AdditionalRandomness = !_param.Deterministic ? additionalRandomness : null,      // Only used for non-deterministic
            Context = _param.SignatureInterface == SignatureInterface.External ? context : null,    // Only used for external
            Message = message,
            Signature = new BitString(signature)
        });
    }
}

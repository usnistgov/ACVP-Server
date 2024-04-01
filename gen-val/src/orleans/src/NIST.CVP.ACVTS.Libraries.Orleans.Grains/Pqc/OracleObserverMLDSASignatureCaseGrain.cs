using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
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
    
    public OracleObserverMLDSASignatureCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IRandom800_90 rand
    ) : base(nonOrleansScheduler)
    {
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

        // Need testable entropy for deterministic = false, and a seed is used
        var testableEntropy = new TestableEntropyProvider(true);
        var seed = _param.Deterministic ? null : _rand.GetRandomBitString(256);
        testableEntropy.AddEntropy(seed);
        
        var dilithiumFactory = new DilithiumFactory(new NativeShaFactory(), testableEntropy);

        var mldsa = dilithiumFactory.GetDilithium(_param.ParameterSet);
        var result = mldsa.Sign(_param.PrivateKey.ToBytes(), message.Bits, _param.Deterministic);
        
        await Notify(new MLDSASignatureResult
        {
            Message = message,
            Signature = new BitString(result),
            Rnd = seed      // Only used for deterministic
        });
    }
}

using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Dilithium;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverMLDSAKeyCaseGrain : ObservableOracleGrainBase<MLDSAKeyPairResult>, IOracleObserverMLDSAKeyCaseGrain
{
    private readonly IEntropyProvider _entropyProvider;
    private readonly IShaFactory _shaFactory;

    private MLDSAKeyGenParameters _param;
    
    public OracleObserverMLDSAKeyCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IEntropyProviderFactory entropyProviderFactory,
        IShaFactory shaFactory
    ) : base(nonOrleansScheduler)
    {
        _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        _shaFactory = shaFactory;
    }

    public async Task<bool> BeginWorkAsync(MLDSAKeyGenParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }
    
    protected override async Task DoWorkAsync()
    {
        var dilithium = new Dilithium(_param.ParameterSet, _shaFactory);

        var seed = _entropyProvider.GetEntropy(256).Bits;
        var key = dilithium.GenerateKey(seed);
        
        var result = new MLDSAKeyPairResult
        {
            Seed = new BitString(seed),
            PublicKey = new BitString(key.pk),
            PrivateKey = new BitString(key.sk)
        };

        await Notify(result);
    }
}

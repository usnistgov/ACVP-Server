using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Kyber;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverMLKEMKeyCaseGrain : ObservableOracleGrainBase<MLKEMKeyPairResult>, IOracleObserverMLKEMKeyCaseGrain
{
    private readonly IShaFactory _shaFactory;
    private readonly IEntropyProvider _entropyProvider;
    
    private MLKEMKeyGenParameters _param;
    
    public OracleObserverMLKEMKeyCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IShaFactory shaFactory,
        IEntropyProviderFactory entropyProviderFactory
    ) : base(nonOrleansScheduler)
    {
        _shaFactory = shaFactory;
        _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
    }
    
    public async Task<bool> BeginWorkAsync(MLKEMKeyGenParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var kyberFactory = new KyberFactory(_shaFactory);
        var kyber = kyberFactory.GetKyber(_param.ParameterSet);

        var seedZ = _entropyProvider.GetEntropy(256).ToBytes(); // 32 bytes
        var seedD = _entropyProvider.GetEntropy(256).ToBytes(); // 32 bytes

        var key = kyber.GenerateKey(seedZ, seedD);

        var result = new MLKEMKeyPairResult
        {
            SeedZ = new BitString(seedZ),
            SeedD = new BitString(seedD),
            EncapsulationKey = new BitString(key.ek),
            DecapsulationKey = new BitString(key.dk)
        };

        await Notify(result);
    }
}

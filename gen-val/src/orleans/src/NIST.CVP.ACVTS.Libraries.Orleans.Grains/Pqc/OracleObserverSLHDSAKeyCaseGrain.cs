using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverSLHDSAKeyCaseGrain : ObservableOracleGrainBase<SLHDSAKeyPairResult>, IOracleObserverSLHDSAKeyCaseGrain
{
    private readonly IEntropyProvider _entropyProvider;
    private readonly IShaFactory _shaFactory;

    private SLHDSAKeyGenParameters _param;
    
    public OracleObserverSLHDSAKeyCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IEntropyProviderFactory entropyProviderFactory,
        IShaFactory shaFactory
    ) : base(nonOrleansScheduler)
    {
        _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        _shaFactory = shaFactory;
    }
    
    public async Task<bool> BeginWorkAsync(SLHDSAKeyGenParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        // pull back all of the attribute values associated with the SLHDSA parameter set in use
        var slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(_param.SlhdsaParameterSet);
        
        // Doesn't need EntropyProvider in this case
        var slhdsa = new Slhdsa(slhdsaParameterSetAttributes, _shaFactory);

        var SKSeed = _entropyProvider.GetEntropy(slhdsaParameterSetAttributes.N * 8).ToBytes();
        var SKPrf = _entropyProvider.GetEntropy(slhdsaParameterSetAttributes.N * 8).ToBytes();
        var PKSeed = _entropyProvider.GetEntropy(slhdsaParameterSetAttributes.N * 8).ToBytes();

        var keyPair = slhdsa.SlhKeyGen(SKSeed, SKPrf, PKSeed);

        var result = new SLHDSAKeyPairResult
        {
            SKSeed = new BitString(SKSeed),
            SKPrf = new BitString(SKPrf),
            PKSeed = new BitString(PKSeed),
            PrivateKey = new BitString(keyPair.PrivateKey.GetBytes()),
            PublicKey = new BitString(keyPair.PublicKey.GetBytes())
        };

        await Notify(result);
    }
}

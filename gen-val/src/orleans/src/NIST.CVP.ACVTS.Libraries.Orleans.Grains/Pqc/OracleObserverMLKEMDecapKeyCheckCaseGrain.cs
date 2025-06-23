using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Kyber;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverMLKEMDecapKeyCheckCaseGrain : ObservableOracleGrainBase<MLKEMKeyPairResult>, IOracleObserverMLKEMDecapKeyCheckCaseGrain
{
    private readonly IShaFactory _shaFactory;
    private readonly IEntropyProvider _entropyProvider;
    
    private MLKEMKeyGenParameters _param;
    
    public OracleObserverMLKEMDecapKeyCheckCaseGrain(
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

        (byte[] ek, byte[] dk) key = kyber.GenerateKey(seedZ, seedD);

        switch (_param.DecapDisposition)
        {
            case MLKEMDecapsulationKeyDisposition.None:
                // No modification, leave dk as-is
                break;
            
            case MLKEMDecapsulationKeyDisposition.ModifyH:
                // Modify the values in dk so that ek || H(ek) no longer matches
                key.dk = new BadDecapsulationKeyManipulator(kyber as Kyber).ManipulateDecapsulationKey(key.dk);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
        
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

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

public class OracleObserverMLKEMDecapCaseGrain : ObservableOracleGrainBase<MLKEMEncapsulationResult>, IOracleObserverMLKEMDecapCaseGrain
{
    private readonly IShaFactory _shaFactory;
    private readonly IEntropyProvider _entropyProvider;
    
    private MLKEMDecapsulationParameters _param;
    
    public OracleObserverMLKEMDecapCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IShaFactory shaFactory,
        IEntropyProviderFactory entropyProviderFactory
    ) : base(nonOrleansScheduler)
    {
        _shaFactory = shaFactory;
        _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
    }

    public async Task<bool> BeginWorkAsync(MLKEMDecapsulationParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }
    
    protected override async Task DoWorkAsync()
    {
        // Generate a test case for an IUT to exercise decapsulation with potential errors introduced
        var kyber = new KyberFactory(_shaFactory).GetKyber(_param.ParameterSet);

        var seedM = _entropyProvider.GetEntropy(256).ToBytes(); // 32 bytes
        var result = kyber.Encapsulate(_param.EncapsulationKey.ToBytes(), seedM);

        var sharedKey = new BitString(result.K);
        var ciphertext = new BitString(result.c);
        
        switch (_param.Disposition)
        {
            case MLKEMDecapsulationDisposition.None:
                // No change
                break;
            
            case MLKEMDecapsulationDisposition.ModifyCiphertext:
                // Flip the last bit in the ciphertext
                ciphertext.Bits.Set(0, !ciphertext.Bits.Get(0));
                break;
        }
        
        await Notify(new MLKEMEncapsulationResult
        {
            Ciphertext = ciphertext,
            SeedM = new BitString(seedM),
            SharedKey = sharedKey,
        });
    }
}

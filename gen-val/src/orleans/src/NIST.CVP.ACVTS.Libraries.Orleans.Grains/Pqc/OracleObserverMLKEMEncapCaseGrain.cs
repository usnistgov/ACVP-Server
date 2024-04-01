using System;
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

public class OracleObserverMLKEMEncapCaseGrain : ObservableOracleGrainBase<MLKEMEncapsulationResult>, IOracleObserverMLKEMEncapCaseGrain
{
    private MLKEMEncapsulationParameters _param;

    private readonly IShaFactory _shaFactory;
    private readonly IEntropyProvider _entropyProvider;
    
    public OracleObserverMLKEMEncapCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IShaFactory shaFactory,
        IEntropyProviderFactory entropyProviderFactory
    ) : base(nonOrleansScheduler)
    {
        _shaFactory = shaFactory;
        _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
    }

    public async Task<bool> BeginWorkAsync(MLKEMEncapsulationParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }
    
    protected override async Task DoWorkAsync()
    {
        // Generate an encapsulation output (really only used for samples, this could be taken from the DecapCaseGrain with Disposition = None)
        
        var kyber = new KyberFactory(_shaFactory).GetKyber(_param.ParameterSet);

        var seedM = _entropyProvider.GetEntropy(256).ToBytes(); // 32 bytes
        var seedMBitString = new BitString(seedM);  // SeedM value gets wiped during encapsulate, so we need to store it first
        var result = kyber.Encapsulate(_param.EncapsulationKey.ToBytes(), seedM);
        
        await Notify(new MLKEMEncapsulationResult
        {
            Ciphertext = new BitString(result.c),
            SeedM = seedMBitString,
            SharedKey = new BitString(result.K),
        });
    }
}

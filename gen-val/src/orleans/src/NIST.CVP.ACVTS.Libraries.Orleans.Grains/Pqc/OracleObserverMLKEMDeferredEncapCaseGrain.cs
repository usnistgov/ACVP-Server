using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Kyber;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverMLKEMDeferredEncapCaseGrain : ObservableOracleGrainBase<MLKEMDecapsulationResult>, IOracleObserverMLKEMDeferredEncapCaseGrain
{
    private MLKEMDecapsulationParameters _param;
    private MLKEMEncapsulationResult _providedResult;
    private readonly IShaFactory _shaFactory;

    public OracleObserverMLKEMDeferredEncapCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IShaFactory shaFactory
    ) : base(nonOrleansScheduler)
    {
        _shaFactory = shaFactory;
    }

    public async Task<bool> BeginWorkAsync(MLKEMDecapsulationParameters param, MLKEMEncapsulationResult providedResult)
    {
        _param = param;
        _providedResult = providedResult;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        // Take a IUT provided ciphertext and determine if it decapsulates into a valid key
        var kyber = new KyberFactory(_shaFactory).GetKyber(_param.ParameterSet);

        var result = kyber.Decapsulate(_param.DecapsulationKey.ToBytes(), _providedResult.Ciphertext.ToBytes());
        
        // Notify observers of result
        await Notify(new MLKEMDecapsulationResult
        {
            ImplicitRejection = result.implicitRejection,
            SharedKey = new BitString(result.sharedKey)
        });
    }
}

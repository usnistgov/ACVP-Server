using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Dilithium;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverMLDSACompleteDeferredSignatureCaseGrain : ObservableOracleGrainBase<MLDSAVerificationResult>, IOracleObserverMLDSACompleteDeferredSignatureCaseGrain
{
    private MLDSASignatureParameters _param;
    private MLDSASignatureResult _providedResult;

    private readonly IEntropyProvider _entropyProvider;
    private readonly IShaFactory _shaFactory;
    
    public OracleObserverMLDSACompleteDeferredSignatureCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IEntropyProviderFactory entropyProviderFactory,
        IShaFactory shaFactory
    ) : base(nonOrleansScheduler)
    {
        _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        _shaFactory = shaFactory;
    }


    public async Task<bool> BeginWorkAsync(MLDSASignatureParameters param, MLDSASignatureResult providedResult)
    {
        _param = param;
        _providedResult = providedResult;
        
        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }
    
    protected override async Task DoWorkAsync()
    {
        var dilithiumFactory = new DilithiumFactory(_shaFactory, _entropyProvider);
        var mldsa = dilithiumFactory.GetDilithium(_param.ParameterSet);
        var result = mldsa.Verify(_providedResult.PublicKey.ToBytes(), _providedResult.Signature.ToBytes(), _providedResult.Message.Bits);
        
        // Notify observers of result
        await Notify(new MLDSAVerificationResult(result ? null : "Unable to verify signature")); // Awkward formatting because verification result expects empty constructor or string
    }
}

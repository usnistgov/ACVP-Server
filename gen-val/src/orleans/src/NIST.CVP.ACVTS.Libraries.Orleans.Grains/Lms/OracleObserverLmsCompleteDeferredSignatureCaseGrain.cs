using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Lms;

public class OracleObserverLmsCompleteDeferredSignatureCaseGrain : ObservableOracleGrainBase<LmsVerificationResult>, IOracleObserverLmsCompleteDeferredSignatureCaseGrain
{
    private readonly ILmsVerifier _lmsVerifier;
    
    private LmsSignatureParameters _param;
    private LmsSignatureResult _providedResult;
    
    public OracleObserverLmsCompleteDeferredSignatureCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        ILmsVerifier lmsVerifier
    ) : base(nonOrleansScheduler)
    {
        _lmsVerifier = lmsVerifier;
    }

    public async Task<bool> BeginWorkAsync(LmsSignatureParameters param, LmsSignatureResult providedResult)
    {
        _param = param;
        _providedResult = providedResult;
        
        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }
    
    protected override async Task DoWorkAsync()
    {
        var result = _lmsVerifier.Verify(_providedResult.PublicKey.ToBytes(), _providedResult.Signature.ToBytes(), _providedResult.Message.ToBytes());
        
        // Notify observers of result
        await Notify(result);
    }
}

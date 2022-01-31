using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms
{
    public interface IOracleObserverLmsVerifySignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<LmsSignatureResult>>
    {
        Task<bool> BeginWorkAsync(LmsSignatureParameters param, LmsKeyResult key, LmsKeyResult badKey);
    }
}

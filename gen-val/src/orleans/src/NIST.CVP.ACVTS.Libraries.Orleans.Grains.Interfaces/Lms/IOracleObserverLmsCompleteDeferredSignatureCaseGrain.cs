using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms;

public interface IOracleObserverLmsCompleteDeferredSignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<LmsVerificationResult>
{
    Task<bool> BeginWorkAsync(LmsSignatureParameters param, LmsSignatureResult providedResult);
}

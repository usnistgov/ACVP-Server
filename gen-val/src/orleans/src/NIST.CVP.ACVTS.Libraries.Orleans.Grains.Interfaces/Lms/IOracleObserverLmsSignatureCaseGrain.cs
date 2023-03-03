using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms;

public interface IOracleObserverLmsSignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<LmsSignatureResult>
{
    Task<bool> BeginWorkAsync(LmsSignatureParameters param);
}

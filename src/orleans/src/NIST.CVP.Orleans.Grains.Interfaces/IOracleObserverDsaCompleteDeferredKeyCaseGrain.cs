using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IOracleObserverDsaCompleteDeferredKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<DsaKeyResult>>
    {
        Task<bool> BeginWorkAsync(DsaKeyParameters param, DsaKeyResult fullParam);
    }
}
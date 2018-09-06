using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Rsa
{
    public interface IOracleObserverRsaCompleteDeferredKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<RsaKeyResult>
    {
        Task<bool> BeginWorkAsync(RsaKeyParameters param, RsaKeyResult fullParam);
    }
}
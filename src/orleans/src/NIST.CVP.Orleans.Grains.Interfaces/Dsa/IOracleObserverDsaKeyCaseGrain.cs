using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Dsa
{
    public interface IOracleObserverDsaKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<DsaKeyResult>
    {
        Task<bool> BeginWorkAsync(DsaKeyParameters param);
    }
}
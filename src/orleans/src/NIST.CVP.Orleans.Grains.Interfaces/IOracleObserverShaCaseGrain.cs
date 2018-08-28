using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IOracleObserverShaCaseGrain : IGrainWithGuidKey, IGrainObservable<HashResult>
    {
        Task<bool> BeginWorkAsync(ShaParameters param);
    }
}

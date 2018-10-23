using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kdf
{
    public interface IOracleObserverTpmKdfCaseGrain : IGrainWithGuidKey, IGrainObservable<TpmKdfResult>
    {
        Task<bool> BeginWorkAsync();
    }
}
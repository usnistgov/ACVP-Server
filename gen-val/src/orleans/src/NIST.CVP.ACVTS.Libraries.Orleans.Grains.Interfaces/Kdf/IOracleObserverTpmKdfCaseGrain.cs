using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf
{
    public interface IOracleObserverTpmKdfCaseGrain : IGrainWithGuidKey, IGrainObservable<TpmKdfResult>
    {
        Task<bool> BeginWorkAsync();
    }
}

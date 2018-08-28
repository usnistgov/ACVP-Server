using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IOracleObserverAesXtsCaseGrain : IGrainWithGuidKey, IGrainObservable<AesXtsResult>
    {
        Task<bool> BeginWorkAsync(AesXtsParameters param);
    }
}
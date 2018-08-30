using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Mac
{
    public interface IOracleObserverHmacCaseGrain : IGrainWithGuidKey, IGrainObservable<MacResult>
    {
        Task<bool> BeginWorkAsync(HmacParameters param);
    }
}
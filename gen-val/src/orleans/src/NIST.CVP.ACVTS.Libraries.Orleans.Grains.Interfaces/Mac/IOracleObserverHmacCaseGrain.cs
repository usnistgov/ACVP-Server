using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Mac
{
    public interface IOracleObserverHmacCaseGrain : IGrainWithGuidKey, IGrainObservable<MacResult>
    {
        Task<bool> BeginWorkAsync(HmacParameters param);
    }
}

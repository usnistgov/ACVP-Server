using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Tdes
{
    public interface IOracleObserverTdesDeferredCounterCaseGrain : IGrainWithGuidKey, IGrainObservable<TdesResult>
    {
        Task<bool> BeginWorkAsync(CounterParameters<TdesParameters> param);
    }
}
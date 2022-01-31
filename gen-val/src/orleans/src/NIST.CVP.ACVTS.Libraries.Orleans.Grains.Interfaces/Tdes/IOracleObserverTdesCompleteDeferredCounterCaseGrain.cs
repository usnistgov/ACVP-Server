using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Tdes
{
    public interface IOracleObserverTdesCompleteDeferredCounterCaseGrain : IGrainWithGuidKey, IGrainObservable<TdesResult>
    {
        Task<bool> BeginWorkAsync(CounterParameters<TdesParameters> param);
    }
}

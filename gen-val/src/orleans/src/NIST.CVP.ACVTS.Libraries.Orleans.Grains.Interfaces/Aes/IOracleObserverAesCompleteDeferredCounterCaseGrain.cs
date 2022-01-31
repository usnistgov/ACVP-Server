using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aes
{
    public interface IOracleObserverAesCompleteDeferredCounterCaseGrain : IGrainWithGuidKey, IGrainObservable<AesResult>
    {
        Task<bool> BeginWorkAsync(CounterParameters<AesParameters> param);
    }
}

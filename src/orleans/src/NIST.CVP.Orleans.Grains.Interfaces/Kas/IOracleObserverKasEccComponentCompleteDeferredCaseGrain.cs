using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas
{
    public interface IOracleObserverKasEccComponentCompleteDeferredCaseGrain : IGrainWithGuidKey, IGrainObservable<KasEccComponentDeferredResult>
    {
        Task<bool> BeginWorkAsync(KasEccComponentDeferredParameters param);
    }
}
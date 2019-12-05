using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas
{
    public interface IOracleObserverKasCompleteDeferredAftIfcCaseGrain : IGrainWithGuidKey, IGrainObservable<KasAftDeferredResult>
    {
        Task<bool> BeginWorkAsync(KasAftDeferredParametersIfc param);
    }
}
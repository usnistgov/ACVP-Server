using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Br2
{
    public interface IOracleObserverKasCompleteDeferredAftIfcCaseGrain : IGrainWithGuidKey, IGrainObservable<KasAftDeferredResult>
    {
        Task<bool> BeginWorkAsync(KasAftDeferredParametersIfc param);
    }
}
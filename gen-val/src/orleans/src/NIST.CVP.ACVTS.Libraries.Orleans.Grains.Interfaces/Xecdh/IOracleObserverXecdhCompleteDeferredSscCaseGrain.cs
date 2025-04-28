using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xecdh
{
    public interface IOracleObserverXecdhCompleteDeferredSscCaseGrain : IGrainWithGuidKey, IGrainObservable<XecdhSscDeferredResult>
    {
        Task<bool> BeginWorkAsync(XecdhSscDeferredParameters param);
    }
}

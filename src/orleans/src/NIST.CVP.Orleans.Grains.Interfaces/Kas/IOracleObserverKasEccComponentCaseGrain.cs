using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas
{
    public interface IOracleObserverKasEccComponentCaseGrain : IGrainWithGuidKey, IGrainObservable<KasEccComponentResult>
    {
        Task<bool> BeginWorkAsync(KasEccComponentParameters param);
    }
}
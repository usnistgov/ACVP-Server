using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas
{
    public interface IOracleObserverKasAftFfcCaseGrain : IGrainWithGuidKey, IGrainObservable<KasAftResultFfc>
    {
        Task<bool> BeginWorkAsync(KasAftParametersFfc param);
    }
}
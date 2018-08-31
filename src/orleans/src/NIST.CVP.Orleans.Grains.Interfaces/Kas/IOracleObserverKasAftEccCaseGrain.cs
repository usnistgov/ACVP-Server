using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas
{
    public interface IOracleObserverKasAftEccCaseGrain : IGrainWithGuidKey, IGrainObservable<KasAftResultEcc>
    {
        Task<bool> BeginWorkAsync(KasAftParametersEcc param);
    }
}
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Tdes
{
    public interface IOracleObserverTdesMctCaseGrain : IGrainWithGuidKey, IGrainObservable<MctResult<TdesResult>>
    {
        Task<bool> BeginWorkAsync(TdesParameters param);
    }
}
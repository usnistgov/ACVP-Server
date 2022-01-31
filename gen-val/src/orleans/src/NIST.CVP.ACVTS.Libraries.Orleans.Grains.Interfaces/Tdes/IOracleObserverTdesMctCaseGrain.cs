using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Tdes
{
    public interface IOracleObserverTdesMctCaseGrain : IGrainWithGuidKey, IGrainObservable<MctResult<TdesResult>>
    {
        Task<bool> BeginWorkAsync(TdesParameters param);
    }
}

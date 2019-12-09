using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Cshake
{
    public interface IOracleObserverCShakeMctCaseGrain : IGrainWithGuidKey, IGrainObservable<MctResult<CShakeResult>>
    {
        Task<bool> BeginWorkAsync(CShakeParameters param);
    }
}
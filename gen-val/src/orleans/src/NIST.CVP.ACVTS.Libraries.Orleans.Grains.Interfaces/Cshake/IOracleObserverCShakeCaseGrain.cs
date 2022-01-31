using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Cshake
{
    public interface IOracleObserverCShakeCaseGrain : IGrainWithGuidKey, IGrainObservable<CShakeResult>
    {
        Task<bool> BeginWorkAsync(CShakeParameters param);
    }
}

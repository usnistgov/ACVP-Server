using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.KeyWrap
{
    public interface IOracleObserverKeyWrapCaseGrain : IGrainWithGuidKey, IGrainObservable<KeyWrapResult>
    {
        Task<bool> BeginWorkAsync(KeyWrapParameters param);
    }
}

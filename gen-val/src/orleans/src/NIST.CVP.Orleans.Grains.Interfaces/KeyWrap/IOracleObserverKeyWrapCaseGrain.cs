using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.KeyWrap
{
    public interface IOracleObserverKeyWrapCaseGrain : IGrainWithGuidKey, IGrainObservable<KeyWrapResult>
    {
        Task<bool> BeginWorkAsync(KeyWrapParameters param);
    }
}
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Eddsa
{
    public interface IOracleObserverEddsaVerifyKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<EddsaKeyResult>>
    {
        Task<bool> BeginWorkAsync(EddsaKeyParameters param);
    }
}
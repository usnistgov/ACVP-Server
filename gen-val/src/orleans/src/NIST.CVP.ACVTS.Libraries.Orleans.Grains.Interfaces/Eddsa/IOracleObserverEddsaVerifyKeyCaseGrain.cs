using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Eddsa
{
    public interface IOracleObserverEddsaVerifyKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<EddsaKeyResult>>
    {
        Task<bool> BeginWorkAsync(EddsaKeyParameters param);
    }
}

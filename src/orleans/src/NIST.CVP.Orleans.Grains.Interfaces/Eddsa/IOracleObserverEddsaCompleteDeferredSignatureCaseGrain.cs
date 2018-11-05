using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Eddsa
{
    public interface IOracleObserverEddsaCompleteDeferredSignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<EddsaSignatureResult>>
    {
        Task<bool> BeginWorkAsync(EddsaSignatureParameters param, EddsaSignatureResult fullParam);
    }
}
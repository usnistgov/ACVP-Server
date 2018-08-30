using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IOracleObserverEddsaDeferredSignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<EddsaSignatureResult>
    {
        Task<bool> BeginWorkAsync(EddsaSignatureParameters param);
    }
}
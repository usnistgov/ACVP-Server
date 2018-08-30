using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Ecdsa
{
    public interface IOracleObserverEcdsaVerifyKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<EcdsaKeyResult>>
    {
        Task<bool> BeginWorkAsync(EcdsaKeyParameters param);
    }
}
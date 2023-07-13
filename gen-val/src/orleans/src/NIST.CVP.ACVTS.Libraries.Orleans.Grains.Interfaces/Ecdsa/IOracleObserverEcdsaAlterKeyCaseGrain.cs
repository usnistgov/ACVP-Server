using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ecdsa
{
    public interface IOracleObserverEcdsaAlterKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<EcdsaKeyResult>
    {
        Task<bool> BeginWorkAsync(EcdsaAlterKeyParameters param);
    }
}

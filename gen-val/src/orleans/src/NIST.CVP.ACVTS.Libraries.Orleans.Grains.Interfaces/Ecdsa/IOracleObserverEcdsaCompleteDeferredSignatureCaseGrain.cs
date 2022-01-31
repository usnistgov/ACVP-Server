using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ecdsa
{
    public interface IOracleObserverEcdsaCompleteDeferredSignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<EcdsaSignatureResult>>
    {
        Task<bool> BeginWorkAsync(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam);
    }
}

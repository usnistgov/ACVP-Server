using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ecdsa
{
    public interface IOracleObserverEcdsaDeferredSignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<EcdsaSignatureResult>
    {
        Task<bool> BeginWorkAsync(EcdsaSignatureParameters param);
    }
}

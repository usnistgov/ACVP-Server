using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa
{
    public interface IOracleObserverRsaCompleteDeferredSignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<RsaSignatureResult>>
    {
        Task<bool> BeginWorkAsync(RsaSignatureParameters param, RsaSignatureResult fullParam);
    }
}

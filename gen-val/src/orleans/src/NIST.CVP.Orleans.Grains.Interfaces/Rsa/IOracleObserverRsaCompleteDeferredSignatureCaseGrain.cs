using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Rsa
{
    public interface IOracleObserverRsaCompleteDeferredSignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<RsaSignatureResult>>
    {
        Task<bool> BeginWorkAsync(RsaSignatureParameters param, RsaSignatureResult fullParam);
    }
}
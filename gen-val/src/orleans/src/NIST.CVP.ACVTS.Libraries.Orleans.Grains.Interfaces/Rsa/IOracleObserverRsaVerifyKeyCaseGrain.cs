using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa
{
    public interface IOracleObserverRsaVerifyKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<RsaKeyResult>>
    {
        Task<bool> BeginWorkAsync(RsaKeyResult param);
    }
}

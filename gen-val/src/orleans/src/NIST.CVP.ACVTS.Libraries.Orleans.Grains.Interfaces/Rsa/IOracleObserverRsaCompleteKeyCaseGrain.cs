using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa
{
    public interface IOracleObserverRsaCompleteKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<RsaKeyResult>
    {
        Task<bool> BeginWorkAsync(RsaKeyResult param, PrivateKeyModes keyMode);
    }
}

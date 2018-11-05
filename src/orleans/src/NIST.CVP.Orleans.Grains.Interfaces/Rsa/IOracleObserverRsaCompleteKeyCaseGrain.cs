using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Rsa
{
    public interface IOracleObserverRsaCompleteKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<RsaKeyResult>
    {
        Task<bool> BeginWorkAsync(RsaKeyResult param, PrivateKeyModes keyMode);
    }
}
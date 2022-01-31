using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa
{
    public interface IOracleObserverRsaDecryptionPrimitiveCaseGrain : IGrainWithGuidKey, IGrainObservable<RsaDecryptionPrimitiveResult>
    {
        Task<bool> BeginWorkAsync(RsaDecryptionPrimitiveParameters param, KeyResult passingKey);
    }
}

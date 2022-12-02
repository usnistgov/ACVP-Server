using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa
{
    public interface IOracleObserverRsaCompleteDpSp800Br2CaseGrain : IGrainWithGuidKey, IGrainObservable<RsaDecryptionPrimitiveResult>
    {
        Task<bool> BeginWorkAsync(RsaDecryptionPrimitiveParameters param, KeyResult passingKey);
    }
}

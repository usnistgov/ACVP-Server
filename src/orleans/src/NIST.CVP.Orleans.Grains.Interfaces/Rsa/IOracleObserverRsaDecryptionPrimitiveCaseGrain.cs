using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Rsa
{
    public interface IOracleObserverRsaDecryptionPrimitiveCaseGrain : IGrainWithGuidKey, IGrainObservable<RsaDecryptionPrimitiveResult>
    {
        Task<bool> BeginWorkAsync(RsaDecryptionPrimitiveParameters param);
    }
}
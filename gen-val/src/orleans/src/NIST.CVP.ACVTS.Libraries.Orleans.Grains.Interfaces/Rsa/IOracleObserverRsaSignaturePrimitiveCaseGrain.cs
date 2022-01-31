using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa
{
    public interface IOracleObserverRsaSignaturePrimitiveCaseGrain : IGrainWithGuidKey, IGrainObservable<RsaSignaturePrimitiveResult>
    {
        Task<bool> BeginWorkAsync(RsaSignaturePrimitiveParameters param, RsaKeyResult key);
    }
}

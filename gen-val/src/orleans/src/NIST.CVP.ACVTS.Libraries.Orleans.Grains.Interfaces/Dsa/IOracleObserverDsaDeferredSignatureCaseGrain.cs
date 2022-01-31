using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Dsa
{
    public interface IOracleObserverDsaDeferredSignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<DsaSignatureResult>
    {
        Task<bool> BeginWorkAsync(DsaSignatureParameters param);
    }
}

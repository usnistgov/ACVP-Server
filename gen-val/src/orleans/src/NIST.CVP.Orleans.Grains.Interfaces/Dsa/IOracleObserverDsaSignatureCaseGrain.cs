using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Dsa
{
    public interface IOracleObserverDsaSignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<DsaSignatureResult>
    {
        Task<bool> BeginWorkAsync(DsaSignatureParameters param);
    }
}
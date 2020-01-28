using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Aead
{
    public interface IOracleObserverAesCcmCaseGrain : IGrainWithGuidKey, IGrainObservable<AeadResult>
    {
        Task<bool> BeginWorkAsync(AeadParameters param);
    }
}
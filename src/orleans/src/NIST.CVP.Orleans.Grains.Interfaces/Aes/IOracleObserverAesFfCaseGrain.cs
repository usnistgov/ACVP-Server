using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Aes
{
    public interface IOracleObserverAesFfCaseGrain : IGrainWithGuidKey, IGrainObservable<AesResult>
    {
        Task<bool> BeginWorkAsync(AesFfParameters param);
    }
}
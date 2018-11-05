using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Aes
{
    public interface IOracleObserverAesMctCaseGrain : IGrainWithGuidKey, IGrainObservable<MctResult<AesResult>>
    {
        Task<bool> BeginWorkAsync(AesParameters param);
    }
}
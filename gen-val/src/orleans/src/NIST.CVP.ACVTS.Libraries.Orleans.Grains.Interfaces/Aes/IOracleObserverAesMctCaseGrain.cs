using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aes
{
    public interface IOracleObserverAesMctCaseGrain : IGrainWithGuidKey, IGrainObservable<MctResult<AesResult>>
    {
        Task<bool> BeginWorkAsync(AesParameters param);
    }
}

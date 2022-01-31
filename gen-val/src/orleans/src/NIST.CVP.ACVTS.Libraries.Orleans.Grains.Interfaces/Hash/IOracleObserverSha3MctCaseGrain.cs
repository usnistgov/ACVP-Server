using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Hash
{
    public interface IOracleObserverSha3MctCaseGrain : IGrainWithGuidKey, IGrainObservable<MctResult<HashResult>>
    {
        Task<bool> BeginWorkAsync(ShaParameters param);
    }
}

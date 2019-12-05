using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Interfaces.Hash
{
    public interface IOracleObserverSha3CaseGrain : IGrainWithGuidKey, IGrainObservable<HashResult>
    {
        Task<bool> BeginWorkAsync(Sha3Parameters param);
    }
}

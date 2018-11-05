using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Interfaces.Hash
{
    public interface IOracleObserverParallelHashCaseGrain : IGrainWithGuidKey, IGrainObservable<ParallelHashResult>
    {
        Task<bool> BeginWorkAsync(ParallelHashParameters param);
    }
}

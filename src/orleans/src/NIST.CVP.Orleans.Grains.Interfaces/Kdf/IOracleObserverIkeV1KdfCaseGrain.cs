using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kdf
{
    public interface IOracleObserverIkeV1KdfCaseGrain : IGrainWithGuidKey, IGrainObservable<IkeV1KdfResult>
    {
        Task<bool> BeginWorkAsync(IkeV1KdfParameters param);
    }
}
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kdf
{
    public interface IOracleObserverIkeV2KdfCaseGrain : IGrainWithGuidKey, IGrainObservable<IkeV2KdfResult>
    {
        Task<bool> BeginWorkAsync(IkeV2KdfParameters param);
    }
}
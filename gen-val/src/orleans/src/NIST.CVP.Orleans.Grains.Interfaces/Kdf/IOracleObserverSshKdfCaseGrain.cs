using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kdf
{
    public interface IOracleObserverSshKdfCaseGrain : IGrainWithGuidKey, IGrainObservable<SshKdfResult>
    {
        Task<bool> BeginWorkAsync(SshKdfParameters param);
    }
}
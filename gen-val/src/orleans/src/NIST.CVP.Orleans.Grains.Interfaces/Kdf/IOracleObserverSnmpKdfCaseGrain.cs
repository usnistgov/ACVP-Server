using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kdf
{
    public interface IOracleObserverSnmpKdfCaseGrain : IGrainWithGuidKey, IGrainObservable<SnmpKdfResult>
    {
        Task<bool> BeginWorkAsync(SnmpKdfParameters param);
    }
}
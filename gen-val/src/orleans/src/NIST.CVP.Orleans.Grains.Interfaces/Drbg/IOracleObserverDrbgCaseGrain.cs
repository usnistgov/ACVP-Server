using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.DRBG;
using Orleans;
using DrbgResult = NIST.CVP.Common.Oracle.ResultTypes.DrbgResult;

namespace NIST.CVP.Orleans.Grains.Interfaces.Drbg
{
    public interface IOracleObserverDrbgCaseGrain : IGrainWithGuidKey, IGrainObservable<DrbgResult>
    {
        Task<bool> BeginWorkAsync(DrbgParameters param);
    }
}
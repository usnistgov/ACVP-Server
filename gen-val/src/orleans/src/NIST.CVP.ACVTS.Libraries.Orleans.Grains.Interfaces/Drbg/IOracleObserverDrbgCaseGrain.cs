using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using Orleans;
using DrbgResult = NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.DrbgResult;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Drbg
{
    public interface IOracleObserverDrbgCaseGrain : IGrainWithGuidKey, IGrainObservable<DrbgResult>
    {
        Task<bool> BeginWorkAsync(DrbgParameters param);
    }
}

using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Drbg
{
    public interface IOracleObserverBlockCipherDfCaseGrain : IGrainWithGuidKey, IGrainObservable<AesResult>
    {
        Task<bool> BeginWorkAsync(AesParameters param);
    }
}

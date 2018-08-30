using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Aes
{
    public interface IOracleObserverAesCounterExtractIvsCaseGrain : IGrainWithGuidKey, IGrainObservable<CounterResult>
    {
        Task<bool> BeginWorkAsync(AesParameters param, AesResult fullParam);
    }
}
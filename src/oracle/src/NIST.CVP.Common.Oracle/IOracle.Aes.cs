using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        Task<AesResult> GetAesCaseAsync(AesParameters param);
        Task<MctResult<AesResult>> GetAesMctCaseAsync(AesParameters param);
        Task<AesXtsResult> GetAesXtsCaseAsync(AesXtsParameters param);
        Task<AesResult> GetAesFfCaseAsync(AesFfParameters param);

        Task<AesResult> GetDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param);
        Task<AesResult> CompleteDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param);
        Task<CounterResult> ExtractIvsAsync(AesParameters param, AesResult fullParam);

        Task<AesResult> GetAesCaseAsync(AesWithPayloadParameters param);
    }
}

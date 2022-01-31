using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
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
        Task<AesResult> GetAesCounterRfcCaseAsync(CounterParameters<AesParameters> param);
        Task<AesResult> CompleteDeferredAesCounterRfcCaseAsync(AesWithPayloadParameters param);

        Task<AesResult> GetAesCaseAsync(AesWithPayloadParameters param);
    }
}

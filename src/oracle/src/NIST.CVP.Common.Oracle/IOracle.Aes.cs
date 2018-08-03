using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        AesResult GetAesCase(AesParameters param);
        MctResult<AesResult> GetAesMctCase(AesParameters param);
        AesXtsResult GetAesXtsCase(AesXtsParameters param);

        AesResult GetDeferredAesCounterCase(CounterParameters<AesParameters> param);
        AesResult CompleteDeferredAesCounterCase(CounterParameters<AesParameters> param);
        CounterResult ExtractIvs(AesParameters param, AesResult fullParam);


        Task<AesResult> GetAesCaseAsync(AesParameters param);
        Task<MctResult<AesResult>> GetAesMctCaseAsync(AesParameters param);
        Task<AesXtsResult> GetAesXtsCaseAsync(AesXtsParameters param);

        Task<AesResult> GetDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param);
        Task<AesResult> CompleteDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param);
        Task<CounterResult> ExtractIvsAsync(AesParameters param, AesResult fullParam);
    }
}

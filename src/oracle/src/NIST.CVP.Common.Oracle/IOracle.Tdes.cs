using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        TdesResult GetTdesCase(TdesParameters param);
        TdesResultWithIvs GetTdesWithIvsCase(TdesParameters param);

        MctResult<TdesResult> GetTdesMctCase(TdesParameters param);
        MctResult<TdesResultWithIvs> GetTdesMctWithIvsCase(TdesParameters param);

        TdesResult GetDeferredTdesCounterCase(CounterParameters<TdesParameters> param);
        TdesResult CompleteDeferredTdesCounterCase(CounterParameters<TdesParameters> param);
        CounterResult ExtractIvs(TdesParameters param, TdesResult fullParam);

        Task<TdesResult> GetTdesCaseAsync(TdesParameters param);
        Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param);
    }
}

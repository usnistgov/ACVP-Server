using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        Task<TdesResult> GetTdesCaseAsync(TdesParameters param);
        Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param);

        Task<TdesResult> GetDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param);
        Task<TdesResult> CompleteDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param);
        Task<CounterResult> ExtractIvsAsync(TdesParameters param, TdesResult fullParam);
    }
}

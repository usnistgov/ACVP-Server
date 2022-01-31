using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
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

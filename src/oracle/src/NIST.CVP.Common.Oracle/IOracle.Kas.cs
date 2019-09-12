using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param);
        Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param);
        Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersEcc param);
        
        Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param);
        Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param);
        Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersFfc param);

        Task<KasAftResultIfc> GetKasAftTestIfcAsync(KasAftParametersIfc param);
        Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersIfc param);
        
        Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param);
        Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param);
    }
}

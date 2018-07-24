using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        KasValResultEcc GetKasValTestEcc(KasValParametersEcc param);
        KasAftResultEcc GetKasAftTestEcc(KasAftParametersEcc param);
        KasAftDeferredResult CompleteDeferredKasTest(KasAftDeferredParametersEcc param);

        KasValResultFfc GetKasValTestFfc(KasValParametersFfc param);
        KasAftResultFfc GetKasAftTestFfc(KasAftParametersFfc param);
        KasAftDeferredResult CompleteDeferredKasTest(KasAftDeferredParametersFfc param);

        KasEccComponentResult GetKasEccComponentTest(KasEccComponentParameters param);
        KasEccComponentDeferredResult CompleteDeferredKasComponentTest(KasEccComponentDeferredParameters param);


        Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param);
        Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param);
        Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersEcc param);
        
        Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param);
        Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param);
        Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersFfc param);
        
        Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param);
        Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param);
    }
}

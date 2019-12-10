using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Br2;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        Task<KasValResult> GetKasValTestAsync(KasValParameters param);
        Task<KasAftResult> GetKasAftTestAsync(KasAftParameters param);
        Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParameters param);
    
        Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param);
        Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param);
        Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersEcc param);
        
        Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param);
        Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param);
        Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersFfc param);

        Task<KasValResultIfc> GetKasValTestIfcAsync(KasValParametersIfc param);
        Task<KasAftResultIfc> GetKasAftTestIfcAsync(KasAftParametersIfc param);
        Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersIfc param);
        
        Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param);
        Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param);
    }
}

using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        AeadResult GetAesCcmCase(AeadParameters param);
        AeadResult GetAesGcmCase(AeadParameters param);
        AeadResult GetAesXpnCase(AeadParameters param);

        AeadResult GetDeferredAesGcmCase(AeadParameters param);
        AeadResult CompleteDeferredAesGcmCase(AeadParameters param, AeadResult fullParam);
        AeadResult GetDeferredAesXpnCase(AeadParameters param);
        AeadResult CompleteDeferredAesXpnCase(AeadParameters param, AeadResult fullParam);


        Task<AeadResult> GetAesCcmCaseAsync(AeadParameters param);
        Task<AeadResult> GetAesGcmCaseAsync(AeadParameters param);
        Task<AeadResult> GetAesXpnCaseAsync(AeadParameters param);
                       
        Task<AeadResult> GetDeferredAesGcmCaseAsync(AeadParameters param);
        Task<AeadResult> CompleteDeferredAesGcmCaseAsync(AeadParameters param, AeadResult fullParam);
        Task<AeadResult> GetDeferredAesXpnCaseAsync(AeadParameters param);
        Task<AeadResult> CompleteDeferredAesXpnCaseAsync(AeadParameters param, AeadResult fullParam);
    }
}

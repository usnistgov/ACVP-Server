using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        Task<AeadResult> GetAesCcmCaseAsync(AeadParameters param);
        Task<AeadResult> GetAesGcmCaseAsync(AeadParameters param);
        Task<AeadResult> GetAesXpnCaseAsync(AeadParameters param);
                       
        Task<AeadResult> GetDeferredAesGcmCaseAsync(AeadParameters param);
        Task<AeadResult> CompleteDeferredAesGcmCaseAsync(AeadParameters param, AeadResult fullParam);
        Task<AeadResult> GetDeferredAesXpnCaseAsync(AeadParameters param);
        Task<AeadResult> CompleteDeferredAesXpnCaseAsync(AeadParameters param, AeadResult fullParam);
    }
}

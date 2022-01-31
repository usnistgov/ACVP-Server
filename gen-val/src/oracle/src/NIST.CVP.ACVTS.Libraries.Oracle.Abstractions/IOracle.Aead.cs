using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    public partial interface IOracle
    {
        Task<AeadResult> GetAesCcmCaseAsync(AeadParameters param);
        Task<AeadResult> GetEcmaCaseAsync(AeadParameters param);
        Task<AeadResult> GetAesGcmCaseAsync(AeadParameters param);
        Task<AeadResult> GetAesGcmSivCaseAsync(AeadParameters param);
        Task<AeadResult> GetAesXpnCaseAsync(AeadParameters param);

        Task<AeadResult> GetDeferredAesGcmCaseAsync(AeadParameters param);
        Task<AeadResult> CompleteDeferredAesGcmCaseAsync(AeadParameters param, AeadResult fullParam);
        Task<AeadResult> GetDeferredAesXpnCaseAsync(AeadParameters param);
        Task<AeadResult> CompleteDeferredAesXpnCaseAsync(AeadParameters param, AeadResult fullParam);
    }
}

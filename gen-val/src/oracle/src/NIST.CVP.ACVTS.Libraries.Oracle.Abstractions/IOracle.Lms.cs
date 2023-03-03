using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    public partial interface IOracle
    {
        Task<LmsKeyPairResult> GetLmsKeyCaseAsync(LmsKeyPairParameters param);
        Task<LmsSignatureResult> GetDeferredLmsSignatureCaseAsync(LmsSignatureParameters param);
        Task<LmsSignatureResult> GetLmsSignatureCaseAsync(LmsSignatureParameters param);
        Task<LmsVerificationResult> CompleteDeferredLmsSignatureAsync(LmsSignatureParameters param, LmsSignatureResult providedResult);
        Task<VerifyResult<LmsSignatureResult>> GetLmsVerifyResultAsync(LmsSignatureParameters param);
    }
}

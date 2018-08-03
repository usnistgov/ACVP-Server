using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        EcdsaKeyResult GetEcdsaKey(EcdsaKeyParameters param);
        EcdsaKeyResult CompleteDeferredEcdsaKey(EcdsaKeyParameters param, EcdsaKeyResult fullParam);
        VerifyResult<EcdsaKeyResult> GetEcdsaKeyVerify(EcdsaKeyParameters param);

        EcdsaSignatureResult GetDeferredEcdsaSignature(EcdsaSignatureParameters param);
        VerifyResult<EcdsaSignatureResult> CompleteDeferredEcdsaSignature(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam);
        EcdsaSignatureResult GetEcdsaSignature(EcdsaSignatureParameters param);

        VerifyResult<EcdsaSignatureResult> GetEcdsaVerifyResult(EcdsaSignatureParameters param);


        Task<EcdsaKeyResult> GetEcdsaKeyAsync(EcdsaKeyParameters param);
        Task<EcdsaKeyResult> CompleteDeferredEcdsaKeyAsync(EcdsaKeyParameters param, EcdsaKeyResult fullParam);
        Task<VerifyResult<EcdsaKeyResult>> GetEcdsaKeyVerifyAsync(EcdsaKeyParameters param);

        Task<EcdsaSignatureResult> GetDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param);
        Task<VerifyResult<EcdsaSignatureResult>> CompleteDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam);
        Task<EcdsaSignatureResult> GetEcdsaSignatureAsync(EcdsaSignatureParameters param);

        Task<VerifyResult<EcdsaSignatureResult>> GetEcdsaVerifyResultAsync(EcdsaSignatureParameters param);
    }
}

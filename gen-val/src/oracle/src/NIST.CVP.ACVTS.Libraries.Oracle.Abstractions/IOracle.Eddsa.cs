using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    public partial interface IOracle
    {
        Task<EddsaKeyResult> GetEddsaKeyAsync(EddsaKeyParameters param);
        Task<EddsaKeyResult> CompleteDeferredEddsaKeyAsync(EddsaKeyParameters param, EddsaKeyResult fullParam);
        Task<VerifyResult<EddsaKeyResult>> GetEddsaKeyVerifyAsync(EddsaKeyParameters param);

        Task<EddsaSignatureResult> GetDeferredEddsaSignatureAsync(EddsaSignatureParameters param);
        Task<VerifyResult<EddsaSignatureResult>> CompleteDeferredEddsaSignatureAsync(EddsaSignatureParameters param, EddsaSignatureResult fullParam);
        Task<EddsaSignatureResult> GetEddsaSignatureAsync(EddsaSignatureParameters param);

        // Remove if test group gets merged together
        Task<EddsaSignatureResult> GetDeferredEddsaSignatureBitFlipAsync(EddsaSignatureParameters param);
        Task<EddsaSignatureResult> GetEddsaSignatureBitFlipAsync(EddsaSignatureParameters param);
        Task<BitString> GetEddsaMessageBitFlipAsync(EddsaMessageParameters param);

        Task<VerifyResult<EddsaSignatureResult>> GetEddsaVerifyResultAsync(EddsaSignatureParameters param);
    }
}

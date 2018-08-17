using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        Task<EddsaKeyResult> GetEddsaKeyAsync(EddsaKeyParameters param);
        Task<EddsaKeyResult> CompleteDeferredEddsaKeyAsync(EddsaKeyParameters param, EddsaKeyResult fullParam);
        Task<VerifyResult<EddsaKeyResult>> GetEddsaKeyVerifyAsync(EddsaKeyParameters param);

        Task<EddsaSignatureResult> GetDeferredEddsaSignatureAsync(EddsaSignatureParameters param);
        Task<VerifyResult<EddsaSignatureResult>> CompleteDeferredEddsaSignatureAsync(EddsaSignatureParameters param, EddsaSignatureResult fullParam);
        Task<EddsaSignatureResult> GetEddsaSignatureAsync(EddsaSignatureParameters param);

        Task<VerifyResult<EddsaSignatureResult>> GetEddsaVerifyResultAsync(EddsaSignatureParameters param);
    }
}

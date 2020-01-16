using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        Task<EccDomainParameters> GetEcdsaDomainParameterAsync(Curve param);
        
        Task<EcdsaKeyResult> GetEcdsaKeyAsync(EcdsaKeyParameters param);
        Task<EcdsaKeyResult> CompleteDeferredEcdsaKeyAsync(EcdsaKeyParameters param, EcdsaKeyResult fullParam);
        Task<VerifyResult<EcdsaKeyResult>> GetEcdsaKeyVerifyAsync(EcdsaKeyParameters param);

        Task<EcdsaSignatureResult> GetDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param);
        Task<VerifyResult<EcdsaSignatureResult>> CompleteDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam);
        Task<EcdsaSignatureResult> GetEcdsaSignatureAsync(EcdsaSignatureParameters param);

        Task<VerifyResult<EcdsaSignatureResult>> GetEcdsaVerifyResultAsync(EcdsaSignatureParameters param);
    }
}

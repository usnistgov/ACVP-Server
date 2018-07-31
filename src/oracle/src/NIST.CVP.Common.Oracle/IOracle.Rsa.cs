using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        RsaKeyResult GetRsaKey(RsaKeyParameters param);
        RsaKeyResult CompleteKey(RsaKeyResult param, PrivateKeyModes keyMode);
        RsaKeyResult CompleteDeferredRsaKeyCase(RsaKeyParameters param, RsaKeyResult fullParam);
        VerifyResult<RsaKeyResult> GetRsaKeyVerify(RsaKeyResult param);

        RsaSignaturePrimitiveResult GetRsaSignaturePrimitive(RsaSignaturePrimitiveParameters param);

        RsaSignatureResult GetDeferredRsaSignature(RsaSignatureParameters param);
        VerifyResult<RsaSignatureResult> CompleteDeferredRsaSignature(RsaSignatureParameters param, RsaSignatureResult fullParam);
        RsaSignatureResult GetRsaSignature(RsaSignatureParameters param);
        VerifyResult<RsaSignatureResult> GetRsaVerify(RsaSignatureParameters param);


        Task<RsaKeyResult> GetRsaKeyAsync(RsaKeyParameters param);
        Task<RsaKeyResult> CompleteKeyAsync(RsaKeyResult param, PrivateKeyModes keyMode);
        Task<RsaKeyResult> CompleteDeferredRsaKeyCaseAsync(RsaKeyParameters param, RsaKeyResult fullParam);
        Task<VerifyResult<RsaKeyResult>> GetRsaKeyVerifyAsync(RsaKeyResult param);

        Task<RsaSignaturePrimitiveResult> GetRsaSignaturePrimitiveAsync(RsaSignaturePrimitiveParameters param);

        Task<RsaSignatureResult> GetDeferredRsaSignatureAsync(RsaSignatureParameters param);
        Task<VerifyResult<RsaSignatureResult>> CompleteDeferredRsaSignatureAsync(RsaSignatureParameters param, RsaSignatureResult fullParam);
        Task<RsaSignatureResult> GetRsaSignatureAsync(RsaSignatureParameters param);
        Task<VerifyResult<RsaSignatureResult>> GetRsaVerifyAsync(RsaSignatureParameters param);
    }
}

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
    }
}

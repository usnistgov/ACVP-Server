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
        RsaSignatureResult GetRsaSignature();
        VerifyResult<RsaSignatureResult> GetRsaVerify();
    }
}

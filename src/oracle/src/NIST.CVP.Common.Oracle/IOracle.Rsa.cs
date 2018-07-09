using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        RsaKeyResult GetRsaKey(RsaKeyParameters param);
        RsaKeyResult CompleteDeferredRsaKeyCase(RsaKeyResult param);
        VerifyResult<RsaKeyResult> GetRsaKeyVerify(RsaKeyResult param);
        RsaSignatureResult GetRsaSignature();
        VerifyResult<RsaSignatureResult> GetRsaVerify();
    }
}

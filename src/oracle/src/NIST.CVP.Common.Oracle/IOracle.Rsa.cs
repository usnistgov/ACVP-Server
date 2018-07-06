using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        RsaKeyResult GetRsaKey();
        VerifyResult<RsaKeyResult> GetRsaKeyVerify();
        RsaSignatureResult GetRsaSignature();
        VerifyResult<RsaSignatureResult> GetRsaVerify();
    }
}

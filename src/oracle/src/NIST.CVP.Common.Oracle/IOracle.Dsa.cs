using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        DsaDomainParametersResult GetDsaDomainParameters();
        VerifyResult<DsaDomainParametersResult> GetDsaDomainParametersVerify();
        DsaKeyResult GetDsaKey();
        VerifyResult<DsaKeyResult> GetDsaKeyVerify();
        DsaSignatureResult GetDsaSignature();
        VerifyResult<DsaSignatureResult> GetDsaVerifyResult();
    }
}

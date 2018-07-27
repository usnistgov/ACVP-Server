using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        DsaDomainParametersResult GetDsaPQ(DsaDomainParametersParameters param);
        DsaDomainParametersResult GetDsaG(DsaDomainParametersParameters param, DsaDomainParametersResult pqParam);
        DsaDomainParametersResult GetDsaDomainParameters(DsaDomainParametersParameters param);
        VerifyResult<DsaDomainParametersResult> GetDsaPQVerify(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam);
        VerifyResult<DsaDomainParametersResult> GetDsaGVerify(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam);

        DsaDomainParametersResult CompleteDeferredDsaDomainParameters(DsaDomainParametersParameters param, DsaKeyResult fullParam);
        VerifyResult<DsaDomainParametersResult> GetDsaDomainParametersVerify(DsaDomainParametersParameters param);

        DsaKeyResult GetDsaKey(DsaKeyParameters param);
        DsaKeyResult CompleteDeferredDsaKey(DsaKeyParameters param, DsaKeyResult fullParam);
        VerifyResult<DsaKeyResult> GetDsaKeyVerify(DsaKeyParameters param);

        DsaSignatureResult GetDeferredDsaSignature(DsaSignatureParameters param);
        VerifyResult<DsaSignatureResult> CompleteDeferredDsaSignature(DsaSignatureParameters param, DsaSignatureResult fullParam);
        DsaSignatureResult GetDsaSignature(DsaSignatureParameters param);

        VerifyResult<DsaSignatureResult> GetDsaVerifyResult(DsaSignatureParameters param);
    }
}

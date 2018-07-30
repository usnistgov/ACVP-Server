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

        DsaKeyResult GetDsaKey(DsaKeyParameters param);
        VerifyResult<DsaKeyResult> CompleteDeferredDsaKey(DsaKeyParameters param, DsaKeyResult fullParam);

        DsaSignatureResult GetDeferredDsaSignature(DsaSignatureParameters param);
        VerifyResult<DsaSignatureResult> CompleteDeferredDsaSignature(DsaSignatureParameters param, DsaSignatureResult fullParam);
        DsaSignatureResult GetDsaSignature(DsaSignatureParameters param);
    }
}

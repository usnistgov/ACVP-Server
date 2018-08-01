using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;

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


        Task<DsaDomainParametersResult> GetDsaPQAsync(DsaDomainParametersParameters param);
        Task<DsaDomainParametersResult> GetDsaGAsync(DsaDomainParametersParameters param, DsaDomainParametersResult pqParam);
        Task<DsaDomainParametersResult> GetDsaDomainParametersAsync(DsaDomainParametersParameters param);
        Task<VerifyResult<DsaDomainParametersResult>> GetDsaPQVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam);
        Task<VerifyResult<DsaDomainParametersResult>> GetDsaGVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam);

        Task<DsaKeyResult> GetDsaKeyAsync(DsaKeyParameters param);
        Task<VerifyResult<DsaKeyResult>> CompleteDeferredDsaKeyAsync(DsaKeyParameters param, DsaKeyResult fullParam);

        Task<DsaSignatureResult> GetDeferredDsaSignatureAsync(DsaSignatureParameters param);
        Task<VerifyResult<DsaSignatureResult>> CompleteDeferredDsaSignatureAsync(DsaSignatureParameters param, DsaSignatureResult fullParam);
        Task<DsaSignatureResult> GetDsaSignatureAsync(DsaSignatureParameters param);
    }
}

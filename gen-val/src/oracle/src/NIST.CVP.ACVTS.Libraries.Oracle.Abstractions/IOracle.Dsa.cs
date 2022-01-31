using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    public partial interface IOracle
    {
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

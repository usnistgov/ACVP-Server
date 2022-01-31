using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    public partial interface IOracle
    {
        Task<LmsKeyResult> GetLmsKeyCaseAsync(LmsKeyParameters param);
        Task<LmsSignatureResult> GetLmsSignatureCaseAsync(LmsSignatureParameters param);
        Task<MctResult<LmsSignatureResult>> GetLmsMctCaseAsync(LmsSignatureParameters param);
        Task<VerifyResult<LmsSignatureResult>> GetLmsVerifyResultAsync(LmsSignatureParameters param);
    }
}

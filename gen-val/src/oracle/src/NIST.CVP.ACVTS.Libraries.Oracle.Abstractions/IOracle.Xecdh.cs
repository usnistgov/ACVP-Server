using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    public partial interface IOracle
    {
        Task<XecdhKeyResult> GetXecdhKeyAsync(XecdhKeyParameters param);
        Task<XecdhKeyResult> CompleteDeferredXecdhKeyAsync(XecdhKeyParameters param, XecdhKeyResult fullParam);
        Task<VerifyResult<XecdhKeyResult>> GetXecdhKeyVerifyAsync(XecdhKeyParameters param);
        Task<XecdhSscResult> GetXecdhSscTestAsync(XecdhSscParameters param);
        Task<XecdhSscDeferredResult> CompleteDeferredXecdhSscTestAsync(XecdhSscDeferredParameters param);
    }
}

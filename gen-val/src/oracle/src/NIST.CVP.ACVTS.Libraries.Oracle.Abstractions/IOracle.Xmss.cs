using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    public partial interface IOracle
    {
        Task<XmssKeyPairResult> GetXmssKeyCaseAsync(XmssKeyPairParameters param);
        Task<XmssSignatureResult> GetDeferredXmssSignatureCaseAsync(XmssSignatureParameters param);
        Task<XmssSignatureResult> GetXmssSignatureCaseAsync(XmssSignatureParameters param);
        Task<XmssVerificationResult> CompleteDeferredXmssSignatureAsync(XmssSignatureParameters param, XmssSignatureResult providedResult);
        Task<VerifyResult<XmssSignatureResult>> GetXmssVerifyResultAsync(XmssSignatureParameters param);
    }
}

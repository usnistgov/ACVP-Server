using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    public partial interface IOracle
    {
        Task<RsaKeyResult> GetRsaKeyAsync(RsaKeyParameters param);
        Task<RsaKeyResult> CompleteKeyAsync(RsaKeyResult param, PrivateKeyModes keyMode);
        Task<RsaKeyResult> CompleteDeferredRsaKeyCaseAsync(RsaKeyParameters param, RsaKeyResult fullParam);
        Task<VerifyResult<RsaKeyResult>> GetRsaKeyVerifyAsync(RsaKeyResult param);

        Task<RsaSignaturePrimitiveResult> GetRsaSignaturePrimitiveAsync(RsaSignaturePrimitiveParameters param);

        Task<RsaSignatureResult> GetDeferredRsaSignatureAsync(RsaSignatureParameters param);
        Task<VerifyResult<RsaSignatureResult>> CompleteDeferredRsaSignatureAsync(RsaSignatureParameters param, RsaSignatureResult fullParam);
        Task<RsaSignatureResult> GetRsaSignatureAsync(RsaSignatureParameters param);
        Task<VerifyResult<RsaSignatureResult>> GetRsaVerifyAsync(RsaSignatureParameters param);

        Task<RsaDecryptionPrimitiveResult> GetDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param);
        Task<RsaDecryptionPrimitiveResult> CompleteDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param, RsaDecryptionPrimitiveResult fullParam);
        Task<RsaDecryptionPrimitiveResult> GetRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param);
        
        Task<RsaDecryptionPrimitiveResult> GetRsaDecryptionPrimitiveSp800B56Br2Async(RsaDecryptionPrimitiveParameters param);
        
        Task<RsaPrimeResult> GetRsaPrimes(RsaKeyParameters param);
    }
}

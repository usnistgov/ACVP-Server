using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public interface IDsaFfcFactory 
        : IDsaFactory<
            IDsaFfc, 
            FfcDomainParametersGenerateRequest, 
            FfcDomainParametersGenerateResult, 
            FfcDomainParametersValidateRequest,
            FfcDomainParametersValidateResult,
            FfcDomainParameters,
            FfcKeyPairGenerateResult,
            FfcKeyPair,
            FfcKeyPairValidateResult,
            FfcSignature,
            FfcSignatureResult,
            FfcVerificationResult
        >
    {
    }
}
namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC
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

namespace NIST.CVP.Crypto.DSA.ECC
{
    public interface IDsaEcc : IDsa<
        EccDomainParametersGenerateRequest,
        EccDomainParametersGenerateResult,
        EccDomainParametersValidateRequest,
        EccDomainParametersValidateResult,
        EccDomainParameters,
        EccKeyPairGenerateResult,
        EccKeyPair,
        EccKeyPairValidationResult,
        EccSignature,
        EccSignatureResult,
        EccVerificationResult
        >
    {
    }
}
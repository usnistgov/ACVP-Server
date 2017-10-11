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
        EccKeyPairValidateResult,
        EccSignature,
        EccSignatureResult,
        EccVerificationResult
        >
    {
    }
}
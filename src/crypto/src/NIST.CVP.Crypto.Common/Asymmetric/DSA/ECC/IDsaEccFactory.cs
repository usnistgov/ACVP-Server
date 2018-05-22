namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC
{
    public interface IDsaEccFactory
        : IDsaFactory<
            IDsaEcc,
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

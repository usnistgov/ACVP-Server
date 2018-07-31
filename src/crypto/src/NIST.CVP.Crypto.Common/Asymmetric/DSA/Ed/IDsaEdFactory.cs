namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public interface IDsaEdFactory
        : IDsaFactory<
            IDsaEd,
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

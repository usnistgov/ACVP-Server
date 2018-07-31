namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public interface IDsaEdFactory
        : IDsaFactory<
            IDsaEd,
            EdDomainParametersGenerateRequest,
            EdDomainParametersGenerateResult,
            EdDomainParametersValidateRequest,
            EdDomainParametersValidateResult,
            EdDomainParameters,
            EdKeyPairGenerateResult,
            EdKeyPair,
            EdKeyPairValidateResult,
            EdSignature,
            EdSignatureResult,
            EdVerificationResult
        >
    {
    }
}

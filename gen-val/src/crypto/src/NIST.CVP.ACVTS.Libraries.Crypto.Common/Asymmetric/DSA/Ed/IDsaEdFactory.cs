namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed
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

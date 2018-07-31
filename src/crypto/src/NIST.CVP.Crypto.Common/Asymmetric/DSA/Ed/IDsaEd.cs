using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public interface IDsaEd : IDsa<
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
        void AddEntropy(BigInteger entropy);
    }
}
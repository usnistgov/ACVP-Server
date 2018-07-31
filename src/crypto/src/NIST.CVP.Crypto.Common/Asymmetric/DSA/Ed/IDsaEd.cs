using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public interface IDsaEd : IDsa<
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
        void AddEntropy(BigInteger entropy);
    }
}
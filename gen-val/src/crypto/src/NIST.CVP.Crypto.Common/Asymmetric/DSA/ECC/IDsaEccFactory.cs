using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

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
        IDsaEcc GetInstanceForKeys(IEntropyProvider keyEntropy);
        IDsaEcc GetInstanceForKeyVerification();
        IDsaEcc GetInstanceForSignatures(HashFunction hashFunction, NonceProviderTypes nonceProviderTypes, IEntropyProvider entropyProvider = null);
        IDsaEcc GetInstanceForVerification(HashFunction hashFunction);
    }
}

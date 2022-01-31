using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC
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

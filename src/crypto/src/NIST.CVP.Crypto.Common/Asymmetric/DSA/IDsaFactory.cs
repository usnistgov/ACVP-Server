using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA
{
    /// <summary>
    /// Interface for obtaining a <see cref="IDsa"/> with all dependencies.
    /// </summary>
    public interface IDsaFactory<out TDsa, TDomainParametersGenerateRequest, TDomainParametersGenerateResult,
        TDomainParametersValidateRequest, TDomainParametersValidateResult,
        TDsaDomainParameters,
        TKeyPairGenerateResult, TDsaKeyPair, TDsaKeyPairValidationResult,
        TDsaSignature, TDsaSignatureResult, TDsaVerificationResult>
        where TDsa : IDsa<TDomainParametersGenerateRequest, TDomainParametersGenerateResult,
            TDomainParametersValidateRequest, TDomainParametersValidateResult,
            TDsaDomainParameters,
            TKeyPairGenerateResult, TDsaKeyPair, TDsaKeyPairValidationResult,
            TDsaSignature, TDsaSignatureResult, TDsaVerificationResult>
        where TDomainParametersGenerateRequest : IDomainParametersGenerateRequest
        where TDomainParametersGenerateResult : IDomainParametersGenerateResult
        where TDomainParametersValidateRequest : IDomainParametersValidateRequest
        where TDomainParametersValidateResult : IDomainParametersValidateResult
        where TDsaDomainParameters : IDsaDomainParameters
        where TKeyPairGenerateResult : IKeyPairGenerateResult
        where TDsaKeyPair : IDsaKeyPair
        where TDsaKeyPairValidationResult : IKeyPairValidateResult
        where TDsaSignature : IDsaSignature
        where TDsaSignatureResult : IDsaSignatureResult
        where TDsaVerificationResult : IDsaVerificationResult
    {
        /// <summary>
        /// Returns a <see cref="IDsa"/> as a concrete type.
        /// </summary>
        /// <param name="dsaAlgorithm">The Algorithm used for the DSA isntance</param>
        /// <param name="hashFunction">The hash information to be used.</param>
        /// <returns></returns>
        TDsa
            GetInstance(HashFunction hashFunction, EntropyProviderTypes entropyType = EntropyProviderTypes.Random);
    }
}
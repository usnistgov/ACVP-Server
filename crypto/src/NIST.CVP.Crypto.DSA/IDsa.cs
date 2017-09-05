using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.DSA
{
    /// <summary>
    /// Describes functionality for the DSA algorithm.
    /// 
    /// http://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.186-4.pdf
    /// </summary>
    public interface IDsa<
                            TDomainParametersGenerateRequest, TDomainParametersGenerateResult,
                            TDomainParametersValidateRequest, TDomainParametersValidateResult,
                            TDsaDomainParameters, 
                            TKeyPairGenerateResult, TDsaKeyPair, TDsaKeyPairValidationResult
                         >
        where TDomainParametersGenerateRequest : IDomainParametersGenerateRequest
        where TDomainParametersGenerateResult : IDomainParametersGenerateResult
        where TDomainParametersValidateRequest : IDomainParametersValidateRequest
        where TDomainParametersValidateResult : IDomainParametersValidateResult
        where TDsaDomainParameters : IDsaDomainParameters
        where TKeyPairGenerateResult : IKeyPairGenerateResult
        where TDsaKeyPair : IDsaKeyPair
        where TDsaKeyPairValidationResult : IKeyPairValidateResult
    {
        /// <summary>
        /// The Sha instance utilized for Dsa
        /// </summary>
        ISha Sha { get; }
        
        /// <summary>
        /// Generates a set of DSA Domain Parameters with state values included in response.
        /// </summary>
        /// <param name="generateRequest">The parameters used creation of the <see cref="TDsaDomainParameters"/></param>
        /// <returns></returns>
        TDomainParametersGenerateResult GenerateDomainParameters(TDomainParametersGenerateRequest generateRequest);

        /// <summary>
        /// Validates a set of DSA Domain Parameters based on the modes provided in the <see cref="TDomainParametersValidateRequest"/>
        /// </summary>
        /// <param name="domainParameters"></param>
        /// <returns></returns>
        TDomainParametersValidateResult ValidateDomainParameters(TDomainParametersValidateRequest domainParameters);

        /// <summary>
        /// Generates a <see cref="IDsaKeyPair"/> based on a set of <see cref="TDsaDomainParameters"/>
        /// </summary>
        /// <param name="domainParameters">The Domain parameters used to generate the key pair</param>
        /// <returns></returns>
        TKeyPairGenerateResult GenerateKeyPair(TDsaDomainParameters domainParameters);

        /// <summary>
        /// Validates a <see cref="TDsaKeyPair"/> based on a set of <see cref="TDsaDomainParameters"/>
        /// </summary>
        /// <param name="domainParameters">The Domain parameters used in generating the key pair</param>
        /// <param name="keyPair">The DSA key pair</param>
        /// <returns></returns>
        TDsaKeyPairValidationResult ValidateKeyPair(TDsaDomainParameters domainParameters, TDsaKeyPair keyPair);
    }
}
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.DSA
{
    /// <summary>
    /// Describes functionality for the DSA algorithm.
    /// 
    /// http://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.186-4.pdf
    /// </summary>
    public interface IDsa
    {
        /// <summary>
        /// The Sha instance utilized for Dsa
        /// </summary>
        ISha Sha { get; }
        
        /// <summary>
        /// Generates a set of DSA Domain Parameters with state values included in response.
        /// </summary>
        /// <param name="generateRequest">The parameters used creation of the <see cref="IDsaDomainParameters"/></param>
        /// <returns></returns>
        IDomainParametersGenerateResult GenerateDomainParameters(IDomainParametersGenerateRequest generateRequest);
        
        /// <summary>
        /// Sets the domain parameters of the DSA instance
        /// </summary>
        /// <param name="domainParameters">The domain parameters to set</param>
        void SetDomainParameters(IDsaDomainParameters domainParameters);
        
        /// <summary>
        /// Generates a <see cref="IDsaKeyPair"/> based on a set of <see cref="IDsaDomainParameters"/>
        /// </summary>
        /// <param name="domainParameters">The Domain parameters used to generate the key pair</param>
        /// <returns></returns>
        IDsaKeyPair GenerateKeyPair(IDsaDomainParameters domainParameters);
    }
}
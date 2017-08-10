namespace NIST.CVP.Crypto.DSA2
{
    /// <summary>
    /// Describes functionality for the DSA algorithm.
    /// 
    /// http://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.186-4.pdf
    /// </summary>
    public interface IDsa
    {
        /// <summary>
        /// Generates a set of PQG Domain Parameters with state values included in response.
        /// </summary>
        /// <param name="pqgRequest"></param>
        /// <returns></returns>
        PqgGenerateResult GeneratePqg(PqgGenerateRequest pqgRequest);
        /// <summary>
        /// Generates a <see cref="DsaKeyPair"/> based on <see cref="PqgDomainParameters"/>
        /// </summary>
        /// <param name="pqg">The Domain parameters used to generate the key pair</param>
        /// <returns></returns>
        DsaKeyPair GenerateKeyPair(PqgDomainParameters pqg);
    }
}
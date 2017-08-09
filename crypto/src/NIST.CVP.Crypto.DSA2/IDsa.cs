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
    }
}
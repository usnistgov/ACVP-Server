namespace NIST.CVP.Crypto.DSA2
{
    /// <summary>
    /// Parameters for FFC <see cref="IDsa.GenerateDomainParameters"/>
    /// </summary>
    public class FfcDomainParametersGenerateRequest : IDomainParametersGenerateRequest
    {
        /// <summary>
        /// The bit length of the seed
        /// </summary>
        public int SeedLength { get; }
        
        /// <summary>
        /// The P bit length, equiv to L
        /// </summary>
        public int PLength { get; }
        
        /// <summary>
        /// The Q bit length, equiv to N
        /// </summary>
        public int QLength { get; }
        
        /// <summary>
        /// The Hash output bit length
        /// </summary>
        public int HashLength { get; }

        public FfcDomainParametersGenerateRequest(int seedLength, int pLength, int qLength, int hashLength)
        {
            SeedLength = seedLength;
            PLength = pLength;
            QLength = qLength;
            HashLength = hashLength;
        }
    }
}
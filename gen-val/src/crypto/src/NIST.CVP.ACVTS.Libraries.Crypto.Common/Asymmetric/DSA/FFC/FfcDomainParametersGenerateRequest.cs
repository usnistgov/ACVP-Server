using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC
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

        /// <summary>
        /// Index used in the Generator Generator
        /// </summary>
        public BitString Index { get; }

        /// <summary>
        /// Prime Generator method
        /// </summary>
        public PrimeGenMode PrimeGen { get; }

        /// <summary>
        /// Generator Generator method
        /// </summary>
        public GeneratorGenMode GeneratorGen { get; }

        public FfcDomainParametersGenerateRequest(int seedLength, int pLength, int qLength, int hashLength, BitString index, PrimeGenMode primeGen, GeneratorGenMode genGen)
        {
            SeedLength = seedLength;
            PLength = pLength;
            QLength = qLength;
            HashLength = hashLength;
            Index = index;
            PrimeGen = primeGen;
            GeneratorGen = genGen;
        }
    }
}

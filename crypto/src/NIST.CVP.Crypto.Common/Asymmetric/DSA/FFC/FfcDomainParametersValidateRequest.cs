using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC
{
    public class FfcDomainParametersValidateRequest : IDomainParametersValidateRequest
    {
        /// <summary>
        /// PQG
        /// </summary>
        public FfcDomainParameters PqgDomainParameters { get; }

        /// <summary>
        /// The seed used in the construction of <see cref="PqgDomainParameters"/>
        /// </summary>
        public DomainSeed Seed { get; }

        /// <summary>
        /// Number of Candidate <see cref="P"/> values generated.
        /// </summary>
        public Counter Count { get; }

        /// <summary>
        /// Index used to create the Generator
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

        public FfcDomainParametersValidateRequest(FfcDomainParameters pqgDomainParameters, DomainSeed seed, Counter count, BitString index, PrimeGenMode primeGen, GeneratorGenMode genGen)
        {
            PqgDomainParameters = pqgDomainParameters;
            Seed = seed;
            Count = count;
            Index = index;
            PrimeGen = primeGen;
            GeneratorGen = genGen;
        }
    }
}
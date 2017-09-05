using System.Numerics;
using NIST.CVP.Crypto.DSA.FFC.Enums;

namespace NIST.CVP.Crypto.DSA.FFC
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
        public BigInteger Seed { get; }

        /// <summary>
        /// Number of Candidate <see cref="P"/> values generated.
        /// </summary>
        public int Counter { get; }

        /// <summary>
        /// Prime Generator method
        /// </summary>
        public PrimeGenMode PrimeGen { get; }

        /// <summary>
        /// Generator Generator method
        /// </summary>
        public GeneratorGenMode GeneratorGen { get; }

        public FfcDomainParametersValidateRequest(FfcDomainParameters pqgDomainParameters, BigInteger seed, int counter, PrimeGenMode primeGen, GeneratorGenMode genGen)
        {
            PqgDomainParameters = pqgDomainParameters;
            Seed = seed;
            Counter = counter;
            PrimeGen = primeGen;
            GeneratorGen = genGen;
        }
    }
}
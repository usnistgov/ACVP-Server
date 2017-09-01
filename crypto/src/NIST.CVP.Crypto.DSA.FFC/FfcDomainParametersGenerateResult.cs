using System.Numerics;

namespace NIST.CVP.Crypto.DSA.FFC
{
    /// <summary>
    /// The resulting information returned via a FFC <see cref="IDsa.GenerateDomainParameters"/> request.
    /// </summary>
    public class FfcDomainParametersGenerateResult : IDomainParametersGenerateResult
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
        /// Was the generation successful?
        /// </summary>
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Message associated to generation attempt
        /// </summary>
        public string ErrorMessage { get; }

        public FfcDomainParametersGenerateResult(FfcDomainParameters pqgDomainParameters, BigInteger seed, int counter)
        {
            PqgDomainParameters = pqgDomainParameters;
            Seed = seed;
            Counter = counter;
        }

        public FfcDomainParametersGenerateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
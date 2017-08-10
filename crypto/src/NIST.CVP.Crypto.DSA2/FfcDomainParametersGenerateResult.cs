using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA2
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
        /// Domain Parameter H TODO 
        /// </summary>
        public BigInteger H { get; }
        /// <summary>
        /// Number of Candidate <see cref="P"/> values generated.
        /// </summary>
        public int Counter { get; }

        /// <summary>
        /// Was the generation successful?
        /// </summary>
        public bool Success { get; }
        /// <summary>
        /// Message associated to generation attempt
        /// </summary>
        public string Message { get; } = "Success";

        public FfcDomainParametersGenerateResult(FfcDomainParameters pqgDomainParameters, BigInteger seed, BigInteger h, int counter)
        {
            PqgDomainParameters = pqgDomainParameters;
            Seed = seed;
            H = h;
            Counter = counter;
            Success = true;
        }

        public FfcDomainParametersGenerateResult(string message)
        {
            Message = message;
            Success = false;
        }
    }
}
using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA2
{
    /// <summary>
    /// The resulting information returned via a <see cref="IDsa.GeneratePqg"/> request.
    /// </summary>
    public class PqgGenerateResult
    {
        /// <summary>
        /// PQG
        /// </summary>
        public PqgDomainParameters PqgDomainParameters { get; }
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

        public PqgGenerateResult(PqgDomainParameters pqgDomainParameters, BigInteger seed, BigInteger h, int counter)
        {
            PqgDomainParameters = pqgDomainParameters;
            Seed = seed;
            H = h;
            Counter = counter;
            Success = true;
        }

        public PqgGenerateResult(string message)
        {
            Message = message;
            Success = false;
        }
    }
}
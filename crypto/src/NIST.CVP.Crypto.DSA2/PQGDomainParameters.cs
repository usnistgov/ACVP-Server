using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA2
{
    /// <summary>
    /// The PQG Domain Parameters, constructed as a part of <see cref="IDsa.GeneratePqg"/>
    /// </summary>
    public class PqgDomainParameters
    {
        /// <summary>
        /// P / L bit length prime
        /// </summary>
        public BigInteger P { get; }
        /// <summary>
        /// Q / N bit length prime, such that <see cref="P"/> - 1 % q
        /// </summary>
        public BigInteger Q { get; }
        /// <summary>
        /// A number whose multiplicative order mod <see cref="P"/> is <see cref="Q"/>
        /// </summary>
        public BigInteger G { get; }

        public PqgDomainParameters(BigInteger p, BigInteger q, BigInteger g)
        {
            P = p;
            Q = q;
            G = g;
        }
    }
}
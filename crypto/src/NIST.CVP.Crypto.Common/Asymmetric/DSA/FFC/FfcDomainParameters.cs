using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC
{
    /// <summary>
    /// The FFC PQG Domain Parameters, constructed as a part of <see cref="IDsa.GenerateDomainParameters"/>
    /// </summary>
    public class FfcDomainParameters : IDsaDomainParameters
    {
        /// <summary>
        /// P / L bit length prime
        /// </summary>
        public BigInteger P { get; set; }
        
        /// <summary>
        /// Q / N bit length prime, such that <see cref="P"/> - 1 % q
        /// </summary>
        public BigInteger Q { get; set; }
        
        /// <summary>
        /// A number whose multiplicative order mod <see cref="P"/> is <see cref="Q"/>
        /// </summary>
        public BigInteger G { get; set; }

        public FfcDomainParameters()
        {
            
        }

        public FfcDomainParameters(BigInteger p, BigInteger q, BigInteger g)
        {
            P = p;
            Q = q;
            G = g;
        }
    }
}
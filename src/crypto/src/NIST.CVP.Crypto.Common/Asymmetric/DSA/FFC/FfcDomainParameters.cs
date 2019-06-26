using System.Numerics;
using NIST.CVP.Math;

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
        public BitString P { get; set; }
        
        /// <summary>
        /// Q / N bit length prime, such that <see cref="P"/> - 1 % q
        /// </summary>
        public BitString Q { get; set; }
        
        /// <summary>
        /// A number whose multiplicative order mod <see cref="P"/> is <see cref="Q"/>
        /// </summary>
        public BitString G { get; set; }

        public FfcDomainParameters()
        {
            
        }

        public FfcDomainParameters(BitString p, BitString q, BitString g)
        {
            P = p;
            Q = q;
            G = g;
        }
    }
}
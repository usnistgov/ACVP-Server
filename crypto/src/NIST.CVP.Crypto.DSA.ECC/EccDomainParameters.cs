using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccDomainParameters : IDsaDomainParameters
    {
        /// <summary>
        /// Field size
        /// </summary>
        public BigInteger Q { get; }

        /// <summary>
        /// Optional seed of at least 160 bits
        /// </summary>
        public BitString Seed { get; }

        /// <summary>
        /// The polynomail representing the curve
        /// </summary>
        public EccCurveBase E { get; }

        /// <summary>
        /// A point of prime order on the curve
        /// </summary>
        public EccPoint G { get; }

        /// <summary>
        /// Cofactor
        /// </summary>
        public BigInteger H { get; }
    }
}
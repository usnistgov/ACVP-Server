using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccDomainParameters : IDsaDomainParameters
    {
        /// <summary>
        /// Field size, either a q = prime > 3, or q = 2^m where m is prime
        /// </summary>
        // public BigInteger FieldSizeQ { get; }

        /// <summary>
        /// Optional seed of at least 160 bits
        /// </summary>
        public BitString Seed { get; }

        /// <summary>
        /// The polynomial representing the curve, contains a, b and the curve type
        /// </summary>
        public IEccCurve CurveE { get; }

        public EccDomainParameters(IEccCurve e)
        {
            CurveE = e;
        }

        public EccDomainParameters(IEccCurve e, BitString seed)
        {
            CurveE = e;
            Seed = seed;
        }
    }
}
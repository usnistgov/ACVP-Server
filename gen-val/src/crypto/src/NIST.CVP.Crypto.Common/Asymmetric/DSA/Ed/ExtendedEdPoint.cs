using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class ExtendedEdPoint
    {
        /// <summary>
        /// X = x/Z
        /// </summary>
        public BigInteger X { get; set; }

        /// <summary>
        /// Y = y/Z
        /// </summary>
        public BigInteger Y { get; set; }

        /// <summary>
        /// T = XY/Z
        /// </summary>
        public BigInteger T { get; set; }

        /// <summary>
        /// Z is 1 by default for quick transform from affine coordinates
        /// </summary>
        public BigInteger Z { get; set; }

        public ExtendedEdPoint(BigInteger x, BigInteger y, BigInteger t, BigInteger z)
        {
            X = x;
            Y = y;
            T = t;
            Z = z;
        }
    }
}

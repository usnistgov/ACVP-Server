using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccPoint
    {
        /// <summary>
        /// X Coordinate of the point
        /// </summary>
        public BigInteger X { get; }

        /// <summary>
        /// Y Coordinate of the point
        /// </summary>
        public BigInteger Y { get; }

        /// <summary>
        /// Order of the point
        /// </summary>
        public BigInteger N { get; }

        public EccPoint(BigInteger x)
        {
            X = x;
        }

        public EccPoint(BigInteger x, BigInteger y)
        {
            X = x;
            Y = y;
        }

        public EccPoint(BigInteger x, BigInteger y, BigInteger n)
        {
            X = x;
            Y = y;
            N = n;
        }
    }
}

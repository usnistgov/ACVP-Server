using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccPoint
    {
        public bool Infinity { get; } = false;

        /// <summary>
        /// X Coordinate of the point
        /// </summary>
        public BigInteger X { get; }

        /// <summary>
        /// Y Coordinate of the point
        /// </summary>
        public BigInteger Y { get; }

        public EccPoint(string inf)
        {
            Infinity = true;
        }

        public EccPoint(BigInteger x)
        {
            X = x;
        }

        public EccPoint(BigInteger x, BigInteger y)
        {
            X = x;
            Y = y;
        }
    }
}

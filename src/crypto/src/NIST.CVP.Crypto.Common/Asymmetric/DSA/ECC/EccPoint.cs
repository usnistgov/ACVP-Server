using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC
{
    public class EccPoint
    {
        public bool Infinity { get; } = false;

        /// <summary>
        /// X Coordinate of the point
        /// </summary>
        public BigInteger X { get; set; }

        /// <summary>
        /// Y Coordinate of the point
        /// </summary>
        public BigInteger Y { get; set; }

        public EccPoint()
        {
            
        }

        public EccPoint(string inf)
        {
            Infinity = true;
        }

        public EccPoint(BigInteger x, BigInteger y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(EccPoint b)
        {
            // Both points are infinity
            if (this.Infinity && b.Infinity)
            {
                return true;
            }

            // One point is infinity and the other is not
            if (this.Infinity || b.Infinity)
            {
                return false;
            }

            // Compare (x,y) values
            if (this.X == b.X && this.Y == b.Y)
            {
                return true;
            }

            return false;
        } 
    }
}

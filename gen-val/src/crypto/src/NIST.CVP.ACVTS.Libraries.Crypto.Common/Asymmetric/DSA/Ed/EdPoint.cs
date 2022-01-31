using System.Numerics;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdPoint
    {
        /// <summary>
        /// X Coordinate of the point
        /// </summary>
        public BigInteger X { get; set; }

        /// <summary>
        /// Y Coordinate of the point
        /// </summary>
        public BigInteger Y { get; set; }

        public EdPoint()
        {

        }

        public EdPoint(BigInteger x, BigInteger y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(EdPoint b)
        {
            // Compare (x,y) values
            if (this.X == b.X && this.Y == b.Y)
            {
                return true;
            }

            return false;
        }
    }
}

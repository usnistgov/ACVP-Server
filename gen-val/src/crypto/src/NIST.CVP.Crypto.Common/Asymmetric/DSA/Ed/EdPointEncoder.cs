using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using System;
using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdPointEncoder
    {
        public static BitString Encode(EdPoint point, int b)
        {
            var encoding = new BitString(point.Y, b);

            var xBit = new BitString(point.X, b).GetLeastSignificantBits(1);

            var bytes = new byte[b / 8];
            bytes[0] = 1 << 7;
            if (xBit.Equals(BitString.One()))
            {
                encoding = encoding.OR(new BitString(bytes));
            }
            else
            {
                encoding = encoding.AND(new BitString(bytes).NOT());
            }

            return BitString.ReverseByteOrder(encoding);      // switch to little endian
        }

        public static EdPoint Decode(BitString encoded, BigInteger p, BigInteger a, BigInteger d, int b)
        {
            var encodedBits = BitString.ReverseByteOrder(encoded);       // switch to big endian
            var x = encodedBits.GetMostSignificantBits(1).ToPositiveBigInteger();
            var YBits = BitString.ConcatenateBits(BitString.Zero(), encodedBits.GetLeastSignificantBits(b - 1));
            var Y = YBits.ToPositiveBigInteger();

            BigInteger X;
            var u = ((Y * Y) - 1) % p;
            var v = ((d * Y * Y) - a) % p;

            if (p % 4 == 3)
            {
                var w = (u * u * u * v * BigInteger.ModPow(BigInteger.ModPow(u, 5, p) * BigInteger.ModPow(v, 3, p) % p, (p - 3) / 4, p)) % p;
                var vwSquare = (v * ((w * w) % p)) % p;
                if (vwSquare == u)
                {
                    X = w;
                }
                else
                {
                    throw new Exception("Square root does not exist");
                }
            }
            else if (p % 8 == 5)
            {
                var w = (u * v * v * v * BigInteger.ModPow(u * BigInteger.ModPow(v, 7, p), (p - 5) / 8, p)) % p;
                var vwSquare = (v * ((w * w) % p)) % p;
                if (vwSquare == u)
                {
                    X = w;
                }
                else if (vwSquare == (p - u).PosMod(p))
                {
                    X = (w * BigInteger.ModPow(2, (p - 1) / 4, p)) % p;
                }
                else
                {
                    throw new Exception("Square root does not exist");
                }

            }
            else
            {
                // need to use Tonelli-Shanks algorithm in SP800-186 Appendix E
                throw new NotImplementedException("Need to implement Tonelli-Shanks");
            }

            if (X == 0 && x == 1)
            {
                throw new Exception("Point Decode failed");
            }

            if (X % 2 == x)
            {
                return new EdPoint(X, Y);
            }
            else
            {
                return new EdPoint(p - X, Y);
            }
        }
    }
}

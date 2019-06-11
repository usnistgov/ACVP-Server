using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using System.Numerics;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.AES_FF
{
    /// <summary>
    /// Internal methods for AES-FFX.
    ///
    /// There is a concept of "numeral strings" (which are separate from <see cref="BitString"/>),
    /// but can be represented as such.
    ///
    /// It may be incorrect, but for the moment I am representing numeral strings as
    /// <see cref="BitString"/> where each number is 32 bits of the <see cref="BitString"/>
    /// </summary>
    public class AesFfInternals : IAesFfInternals
    {
        public BigInteger Num(int radix, BitString x)
        {
            // 1. Let x = 0.
            BigInteger result = 0;

            // 2. For i from 1 to LEN(X), let x = x*radix + X[i].
            for (var i = 0; i <= x.BitLength - 1; i++)
            {
                result = result * radix + (x.Bits[i] ? 1 : 0);
            }

            // 3. Return x.
            return result;
        }

        public BigInteger Num(BitString x)
        {
            // 1. Let x = 0.
            BigInteger result = 0;

            // 2. For i from 1 to LEN(X), let x = 2x + X[i].
            for (var i = 0; i <= x.BitLength - 1; i++)
            {
                result = 2 * result + (x.Bits[i] ? 1 : 0);
            }

            // 3. Return x.
            return result;
        }

        public BitString Str(int radix, int m, int x)
        {
            var resultArray = new BitString[m];
            var result = new BitString(0);

            // 1. For i from 1 to m:
            for (var i = 0; i <= m - 1; i++)
            {
                // i. X[m+1–i] = x mod radix;
                //resultArray[m - 1 - i] = BitString.To32BitString(x % radix);
                result = result.ConcatenateBits(BitString.To32BitString(x % radix));

                // ii. x = Floor(x/radix).
                x = (int)System.Math.Floor((double)x / radix);
            }

            //foreach (var bs in resultArray)
            //{
            //    result = result.ConcatenateBits(bs);
            //}

            // 2. Return X.
            return result;
        }

        public BitString Rev(BitString x)
        {
            // 1. For i from 1 to LEN(X), let Y[i] = X[LEN(X)+1–i].
            // 2. Return Y[1..LEN(X)].
            return new BitString(MsbLsbConversionHelpers.ReverseBitArrayBits(x.Bits));
        }

        public BitString RevB(BitString x)
        {
            // 1. For i from 0 to BYTELEN(X)–1 and j from 1 to 8, let Y[8i+j] = X[8×(BYTELEN(X)–1–i)+j].
            // 2. Return Y[1..8×BYTELEN(X)].
            return new BitString(MsbLsbConversionHelpers.ReverseByteOrder(x.ToBytes()));
        }

        public BitString Prf(BitString x, BitString key)
        {
            throw new System.NotImplementedException();
        }
    }
}
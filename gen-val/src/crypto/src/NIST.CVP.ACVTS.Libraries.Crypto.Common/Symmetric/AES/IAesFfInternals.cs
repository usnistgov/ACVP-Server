using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES
{
    /// <summary>
    /// Functions internal to AES-FF1 and AES-FF3.
    /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-38Gr1-draft.pdf
    /// </summary>
    public interface IAesFfInternals
    {
        /// <summary>
        /// Algorithm 1 - Num_radix
        ///
        /// The number that the numeral string X represents in base radix when the
        /// numerals are valued in decreasing order of significance. For example,
        /// NUM5 (00011010)=755. An algorithm for computing NUMradix (X) is given
        /// in Sec. 4.5.
        /// </summary>
        /// <param name="radix">The base.</param>
        /// <param name="x">The numeral string X.</param>
        /// <returns></returns>
        BigInteger Num(int radix, NumeralString x);

        /// <summary>
        /// Algorithm 2 - Num
        ///
        /// The integer that a bit string X represents when the bits are valued in
        /// decreasing order of significance. For example, NUM(10000000)=128. An
        ///    algorithm for computing NUM(X) is given in Sec. 4.5.
        /// </summary>
        /// <param name="x">The byte string X.</param>
        /// <returns></returns>
        BigInteger Num(BitString x);

        /// <summary>
        /// Algorithm 3 - Str_m_radix
        ///
        /// Given a nonnegative integer x less than radixm, the representation of x as a
        /// string of m numerals in base radix, in decreasing order of significance. For
        /// example, STR4 12 (559) is the string of four numerals in base 12 that represents
        /// 559, namely, 0 3 10 7. An algorithm for computing STR m radix (x) is given in Sec. 4.5.
        /// </summary>
        /// <param name="radix">The base.</param>
        /// <param name="m">the number of numerals.</param>
        /// <param name="x">The integer X, such that 0 &lt;= x &lt; radix^m  </param>
        /// <returns></returns>
        NumeralString Str(int radix, int m, BigInteger x);

        /// <summary>
        /// Given a numeral string, X, the numeral string that consists of the numerals
        /// of X in reverse order. For example, in base ten, REV(13579) = 97531.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        NumeralString Rev(NumeralString x);

        /// <summary>
        /// Given a byte string, X, the byte string that consists of the bytes of X in
        /// reverse order. For example, REVB([1]1 ||[2]1 ||[3]1)=[3]1 ||[2]1 ||[1]1.
        /// </summary>
        /// <param name="x">The byte string</param>
        /// <returns></returns>
        BitString RevB(BitString x);

        /// <summary>
        /// The output of the function PRF applied to the block X; PRF is defined in terms
        /// of a given designated cipher function.
        /// </summary>
        /// <param name="x">The block string.</param>
        /// <param name="key">The key for use in the cipher function.</param>
        /// <returns></returns>
        BitString Prf(BitString x, BitString key);
    }
}
